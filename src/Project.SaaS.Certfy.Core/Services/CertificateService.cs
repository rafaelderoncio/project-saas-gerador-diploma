using System.Drawing;
using System.Globalization;
using System.Net;
using System.Text;
using Microsoft.AspNetCore.Http;
using Project.SaaS.Certfy.Core.Exceptions;
using Project.SaaS.Certfy.Core.Extensions;
using Project.SaaS.Certfy.Core.Helpers;
using Project.SaaS.Certfy.Core.Models;
using Project.SaaS.Certfy.Core.Services.Interfaces;
using Project.SaaS.Certfy.Core.Validators;
using Project.SaaS.Certfy.Domain.Enums;
using Project.SaaS.Certfy.Domain.Requests;

namespace Project.SaaS.Certfy.Core.Services;

public class CertificateService(
    IPdfService pdfService,
    ICourseService courseService,
    IDisciplineService disciplineService,
    IInstitutionService institutionService,
    IHttpContextAccessor accessor) : ICertificateService
{
    public async Task<byte[]> GenerateAsync(CertificateRequest request)
    {
        try
        {

            var model = new CertificateModel
            {
                ConclusionDate = request.ConclusionDate.ToString(
                    format: "dd 'de' MMMM 'de' yyyy",
                    provider: new CultureInfo("pt-BR")
                )
            };

            // validate
            await ValidateCertificate(request, model);

            // generate hash auth
            model.Authentication = model.ToHash();

            // persist > feature
            // certificateRepository.SaveAsync(model);

            var baseUrl = HostHelper.GetBaseUrl(accessor.HttpContext!);
            var validateUrl = $"{baseUrl}/certificate/validate?authentication={model.Authentication}";
            var qrCode = QrCodeHelper.Generate(validateUrl);
            var qrCodeBase64 = Convert.ToBase64String(qrCode);

            return await GenerateCertificatePDFAsync(model, "default", qrCodeBase64);
        }
        catch (BaseException) { throw; }
        catch (Exception ex)
        {
            throw new BaseException(
                exceptionMessage: ex.Message,
                title: "Erro interno",
                detail: "Erro ao gerar certificado.",
                status: HttpStatusCode.InternalServerError
            );
        }
    }

    private async Task ValidateCertificate(CertificateRequest request, CertificateModel model)
    {
        #region validate-contracts
        var validation = await new CertificateRequestValidator().ValidateAsync(request);

        if (!validation.IsValid)
            throw new BaseException(
                title: "Requisição Inválida",
                detail: $"Erros: {string.Join(';', validation.Errors)}",
                status: HttpStatusCode.BadRequest
            );
        #endregion

        #region validate-institution 
        var institution = await institutionService.GetInstitutionAsync(request.InstitutionId) ??
            throw new BaseException(
                title: "Requisição Inválida",
                detail: $"Instituição com o ID: {request.InstitutionId} não localizada.",
                status: HttpStatusCode.BadRequest
            );

        model.Institution = new InstitutionCertificateModel
        {
            Id = institution.InstitutionId,
            Name = institution.Name,
            Campus = $"{institution.Acronym} - {institution.City}"
        };
        #endregion

        #region validate-course
        var course = await courseService.GetCourseAsync(request.Course.CourseId) ??
            throw new BaseException(
                title: "Requisição Inválida",
                detail: $"Curso com o ID: {request.Course.CourseId} não localizado.",
                status: HttpStatusCode.BadRequest
            );

        model.Course = new CourseCertificateModel
        {
            Id = course.CourseId,
            Name = course.Name,
            Avagage = course.AverageApproval,
            Degree = course.Degree switch
            {
                var value when value == DegreeType.BachelorsDegree.GetDisplayName() => DegreeType.BachelorsDegree,
                var value when value == DegreeType.LicentiateDegree.GetDisplayName() => DegreeType.LicentiateDegree,
                var value when value == DegreeType.TechnologistDegree.GetDisplayName() => DegreeType.TechnologistDegree,
                _ => throw new BaseException(
                    title: "Requisição Inválida",
                    detail: $"Grau '{course.Degree}' não cadastrado.",
                    status: HttpStatusCode.BadRequest
                )
            }
        };
        #endregion

        #region validate-disciplines
        model.Disciplines = [];
        foreach (var item in request.Course.Disciplines)
        {
            if (!model.Disciplines.Any(x => x.Id == item.DisciplineId))
            {
                var discipline = await disciplineService.GetDisciplineAsync(item.DisciplineId) ??
                    throw new BaseException(
                        title: "Requisição Inválida",
                        detail: $"Disciplina com o ID: {item.DisciplineId} não localizada.",
                        status: HttpStatusCode.BadRequest
                    );

                model.Disciplines.Add(new DisciplineCertificateModel
                {
                    Id = discipline.DisciplineId,
                    Name = discipline.Name,
                    Period = item.HasDispensed ? string.Empty : item.Period,
                    Average = item.HasDispensed ? default : item.Average,
                    Workload = discipline.Workload,
                    Status = item.HasDispensed ? StatusType.Dispensed :
                            item.Average >= discipline.AverageApproval ?
                            StatusType.Appoved : StatusType.Repproved
                });
            }
        }

        if (request.Course.Disciplines.DistinctBy(x => x.DisciplineId).Count() != model.Disciplines.Count)
            throw new BaseException(
                title: "Requisição Inválida",
                detail: $"Não é permitido disciplinas repitidas.",
                status: HttpStatusCode.BadRequest
            );
        #endregion

        #region validate-student
        model.Student = new StudentCertificateModel
        {
            Name = request.Student.Name,
            Registration = request.Student.Registration,
            DocumentNumber = request.Student.DocumentNumber,
            DocumentType = request.Student.DocumentType switch
            {
                var value when value == DocumentType.CPF.GetDisplayName() => DocumentType.CPF,
                var value when value == DocumentType.CNH.GetDisplayName() => DocumentType.CNH,
                var value when value == DocumentType.RG.GetDisplayName() => DocumentType.RG,
                _ => throw new BaseException(
                    title: "Requisição Inválida",
                    detail: $"Tipo de documento do aluno '{request.Student.DocumentType}' não cadastrado.",
                    status: HttpStatusCode.BadRequest
                )
            }
        };
        #endregion

        #region validate-signature
        model.Signature = new SignatureCertificateModel
        {
            AdmnistrativePersonName = request.Signature.AdministrativePersonName,
            DeersPersonName = request.Signature.DeersPersonName,
            Date = DateTime.UtcNow.ToString(
                format: "dd 'de' MMMM 'de' yyyy",
                provider: new CultureInfo("pt-BR")
            ),
            Location = $"{institution.City} - {institution.State}"
        };
        #endregion

        #region validate-final-status
        var finalAverage = request.Course.Disciplines
            .Where(x => !x.HasDispensed).Average(x => x.Average);

        model.Status = finalAverage >= course.AverageApproval ?
            StatusType.Appoved : StatusType.Repproved;

        if (model.Status is StatusType.Repproved)
            throw new BaseException(
                title: "Requisição Inválida",
                detail: $"Média para aprovação do curso ({course.AverageApproval}) não foi atingida. Média atingida: {finalAverage}.",
                status: HttpStatusCode.BadRequest
            );
        #endregion
    }

    private async Task<byte[]> GenerateCertificatePDFAsync(CertificateModel model, string template, string? qrCode = null)
    {
        try
        {
            // load template
            var path = Path.Combine(AppContext.BaseDirectory, "Templates", $"{template}.html");
            var content = await FileHelper.LoadContentAsync(path)
                ?? throw new Exception();
                
            StringBuilder html = new (content);

            // set simple data
            html = ProcessSimpleData(model, html);

            // set qrcode
            html = ProcessQrCode(qrCode, html);

            // set disciplines
            html = ProcessDisciplines(model, html);

            // set info text
            html = ProcessFinalAvarage(model, html);

            // proccess 
            return await pdfService.GenerateAsync(html.ToString());
        }
        catch (System.Exception)
        {
            
            throw;
        }
    }

    private static StringBuilder ProcessSimpleData(CertificateModel model, StringBuilder html)
    {
        return html
            .Replace("{{UNIVERSITY_NAME}}", model.Institution.Name)
            .Replace("{{STUDENT_NAME}}", model.Student.Name)
            .Replace("{{COURSE_DEGREE}}", model.Course.Degree.GetDisplayName())
            .Replace("{{COURSE_NAME}}", model.Course.Name)
            .Replace("{{COURSE_COMPLETATION}}", model.ConclusionDate)
            .Replace("{{SIGNATURE_LOCATION}}", model.Signature.Location)
            .Replace("{{SIGNATURE_DATE}}", model.Signature.Date)
            .Replace("{{SIGNATURE_DEERS}}", model.Signature.DeersPersonName)
            .Replace("{{SIGNATURE_ADMINISTRATIVE}}", model.Signature.AdmnistrativePersonName)
            .Replace("{{DIGITAL_AUTHENTICATION}}", model.Authentication)
            .Replace("{{DOCUMENT_TYPE}}", model.Student.DocumentType.GetDisplayName())
            .Replace("{{DOCUMENT_NUMBER}}", model.Student.DocumentNumber)
            .Replace("{{REGISTRATION_NUMBER}}", model.Student.Registration)
            .Replace("{{CAMPUS_NAME}}", model.Institution.Campus)
            .Replace("{{TOTAL_MEDIA}}", model.Disciplines.Average(x => x.Average).ToString("0.0").Replace(".", ","))
            .Replace("{{TOTAL_HOURS}}", model.Disciplines.Sum(x => x.Workload).ToString())
            .Replace("{{COURSE_STATUS}}", model.Status.GetDisplayName());
    }

    private static StringBuilder ProcessQrCode(string? qrCode, StringBuilder html)
    {
        return qrCode is null ?
            html.Replace("''QRCODE_STYLE''", "none") :
            html.Replace("''QRCODE_STYLE''", "block")
                .Replace("{{BASE64_QRCODE}}", qrCode);;
    }

    private static StringBuilder ProcessDisciplines(CertificateModel model, StringBuilder html)
    {
        var groups = ProcessDisciplineGroups(model);
        if (groups.Count > 1)
        {
            var pages = Enumerable.Range(1, groups.Count - 1).Select(index => "{{NEW_PAGE_" + index + "}}");
            html.Replace("{{NEW_PAGE_N}}", string.Join("", pages));
        }

        foreach (var (group, index) in groups.Select((value, index) => (value, index)))
        {
            var disciplinesSb = new StringBuilder();

            if (index == 0)
            {
                ProcessDisciplineGroup(group, disciplinesSb);
                html.Replace("{{DISCIPLINES_TABLE}}", disciplinesSb.ToString());
            }
            else
            {
                string page = GetPage();
                ProcessDisciplineGroup(group, disciplinesSb);
                page = page.Replace("{{DISCIPLINES_TABLE}}", disciplinesSb.ToString());
                html.Replace("{{NEW_PAGE_" + index + "}}", page);
            }
        }

        return html;
    }

    private static StringBuilder ProcessFinalAvarage(CertificateModel model, StringBuilder html)
    {
        html = html.Replace(
                        "{{TEXT_INFO_FINAL_AVARAGE}}",
                        @"
                <div class=""grading-system"">
                    <p><strong>Critérios de Avaliação:</strong> Escala de 0,0 a 10,0. Média mínima para aprovação: {{AVARAGE_APPROVAL}}.</p>
                    <p>Documento gerado eletronicamente em conformidade com a Portaria MEC nº 360/2022.</p>
                </div>"
                    ).Replace(
                        "{{AVARAGE_APPROVAL}}",
                        model.Course.Avagage.ToString("0.0").Replace(".", ",")
                    );
        return html;
    }

    private static void ProcessDisciplineGroup(List<DisciplineCertificateModel> group, StringBuilder disciplinesSb)
    {
        foreach (var discipline in group)
        {
            string row = GetRow(discipline);
            disciplinesSb.AppendLine(row);
        }
    }

    private static string GetPage()
    {
        return @"
                <div class=""page"">
                    <div class=""watermark"" style=""opacity: 0.015;""></div>
                    <div class=""border-outer"" style=""border-width: 1px; border-color: #ccc;""></div>

                    <div class=""content"" style=""padding: 0;"">

                        <div class=""grades-container"">
                            <table class=""grades-table"">
                                <tbody>

                                    {{DISCIPLINES_TABLE}}

                                </tbody>
                            </table>
                        </div>
                    </div>
                </div>
            ";
    }

    private static string GetRow(DisciplineCertificateModel discipline)
    {
        return @"
            <tr>
                <td>{{DISCIPLINE_NAME}}</td>
                <td class=""center-text"">{{DISCIPLINE_PERIOD}}</td>
                <td class=""center-text"">{{DISCIPLINE_MEDIA}}</td>
                <td class=""center-text"">{{DISCIPLINE_HOURS}}</td>
                <td class=""center-text"">{{DISCIPLINE_STATUS}}</td>
            </tr>"
            .Replace("{{DISCIPLINE_NAME}}", discipline.Name)
            .Replace("{{DISCIPLINE_PERIOD}}", discipline.Period)
            .Replace("{{DISCIPLINE_MEDIA}}", discipline.Average.ToString("0.0").Replace(".", ","))
            .Replace("{{DISCIPLINE_HOURS}}", discipline.Workload.ToString())
            .Replace("{{DISCIPLINE_STATUS}}", discipline.Status.GetDisplayName());
    }

    private static List<List<DisciplineCertificateModel>> ProcessDisciplineGroups(CertificateModel model)
    {
        var disciplines = model.Disciplines.ToList();
        var groups = new List<List<DisciplineCertificateModel>>();

        var skipFisrtPage = 6;
        var skipAnotherPages = 20;

        int i = 0;
        groups.Add([.. disciplines.Take(skipFisrtPage)]);
        i = skipFisrtPage;

        while (i < disciplines.Count)
        {
            groups.Add([.. disciplines.Skip(i).Take(skipAnotherPages)]);
            i += skipAnotherPages;
        }

        return groups;
    }

    public Task<object> ValidateAsync(string authentication)
    {
            throw new BaseException(
                title: "Valiar Certificado",
                detail: "Funcionalidade não implementada.",
                status: HttpStatusCode.NoContent
            );
    }
}
