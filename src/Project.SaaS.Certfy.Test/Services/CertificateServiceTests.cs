using System.Net;
using AutoFixture;
using Microsoft.AspNetCore.Http;
using NSubstitute;
using Project.SaaS.Certfy.Core.Exceptions;
using Project.SaaS.Certfy.Core.Services;
using Project.SaaS.Certfy.Core.Services.Interfaces;
using Project.SaaS.Certfy.Domain.Requests;
using Project.SaaS.Certfy.Domain.Responses;
using Project.SaaS.Certfy.Test.Fixtures;

namespace Project.SaaS.Certfy.Test.Services;

public class CertificateServiceTests
{
    private readonly IFixture _fixture = AutoNSubstituteFixture.Create();

    [Fact]
    public async Task GenerateAsync_WhenRequestIsValid_ShouldReturnPdfBytes()
    {
        EnsureTemplateFile();

        var request = CertificateRequestFixture.CreateValidRequest(_fixture);
        var expectedPdf = _fixture.CreateMany<byte>(64).ToArray();
        var (service, pdfService, courseService, disciplineService, institutionService, _) = BuildServiceWithDependencies();

        ConfigureValidDependencies(request, courseService, disciplineService, institutionService);
        pdfService.GenerateAsync(Arg.Any<string>()).Returns(expectedPdf);

        var result = await service.GenerateAsync(request);

        Assert.Equal(expectedPdf, result);
        await pdfService.Received(1).GenerateAsync(Arg.Any<string>());
    }

    [Fact]
    public async Task GenerateAsync_WhenRequestIsInvalid_ShouldThrowBaseExceptionBadRequest()
    {
        var request = CertificateRequestFixture.CreateValidRequest(_fixture);
        request.Course.Disciplines = [];

        var (service, _, _, _, _, _) = BuildServiceWithDependencies();

        var exception = await Assert.ThrowsAsync<BaseException>(() => service.GenerateAsync(request));

        Assert.Equal(HttpStatusCode.BadRequest, exception.Status);
        Assert.Equal("Requisição Inválida", exception.Title);
        Assert.Contains("Course.Disciplines", exception.Detail);
    }

    [Fact]
    public async Task GenerateAsync_WhenDisciplinesAreDuplicated_ShouldThrowBaseExceptionBadRequest()
    {
        var request = CertificateRequestFixture.CreateValidRequest(_fixture);
        request.Course.Disciplines =
        [
            new DisciplineCertificateRequest
            {
                DisciplineId = "disc-dup",
                Period = "2025/1",
                Average = 8.0m,
                HasDispensed = false
            },
            new DisciplineCertificateRequest
            {
                DisciplineId = "disc-dup",
                Period = "2025/2",
                Average = 9.0m,
                HasDispensed = false
            }
        ];

        var (service, _, courseService, disciplineService, institutionService, _) = BuildServiceWithDependencies();
        ConfigureValidDependencies(request, courseService, disciplineService, institutionService);
        disciplineService.GetDisciplineAsync("disc-dup").Returns(new DisciplineResponse
        {
            DisciplineId = "disc-dup",
            Name = "Disciplina Duplicada",
            Description = "Teste",
            Types = ["Tecnologia"],
            Workload = 60,
            AverageApproval = 7.0m
        });

        var exception = await Assert.ThrowsAsync<BaseException>(() => service.GenerateAsync(request));

        Assert.Equal(HttpStatusCode.BadRequest, exception.Status);
        Assert.Equal("Requisição Inválida", exception.Title);
        Assert.Contains("repitidas", exception.Detail);
    }

    [Fact]
    public async Task GenerateAsync_WhenFinalAverageIsBelowCourseMinimum_ShouldThrowBaseExceptionBadRequest()
    {
        var request = CertificateRequestFixture.CreateValidRequest(_fixture);
        request.Course.Disciplines =
        [
            new DisciplineCertificateRequest
            {
                DisciplineId = "disc-low-1",
                Period = "2025/1",
                Average = 7.0m,
                HasDispensed = false
            }
        ];

        var (service, _, courseService, disciplineService, institutionService, _) = BuildServiceWithDependencies();
        ConfigureValidDependencies(request, courseService, disciplineService, institutionService);

        courseService.GetCourseAsync(request.Course.CourseId).Returns(new CourseResponse
        {
            CourseId = request.Course.CourseId,
            Name = "Curso Exigente",
            Degree = "Bacharelado",
            TotalWorkload = 3000,
            AverageApproval = 9.0m
        });

        disciplineService.GetDisciplineAsync("disc-low-1").Returns(new DisciplineResponse
        {
            DisciplineId = "disc-low-1",
            Name = "Disciplina",
            Description = "Teste",
            Types = ["Tecnologia"],
            Workload = 80,
            AverageApproval = 5.0m
        });

        var exception = await Assert.ThrowsAsync<BaseException>(() => service.GenerateAsync(request));

        Assert.Equal(HttpStatusCode.BadRequest, exception.Status);
        Assert.Equal("Requisição Inválida", exception.Title);
        Assert.Contains("não foi atingida", exception.Detail);
    }

    [Fact]
    public async Task GenerateAsync_WhenUnexpectedErrorOccurs_ShouldWrapIntoInternalBaseException()
    {
        var request = CertificateRequestFixture.CreateValidRequest(_fixture);
        var (service, _, courseService, disciplineService, institutionService, accessor) = BuildServiceWithDependencies();

        accessor.HttpContext.Returns((HttpContext?)null);
        ConfigureValidDependencies(request, courseService, disciplineService, institutionService);

        var exception = await Assert.ThrowsAsync<BaseException>(() => service.GenerateAsync(request));

        Assert.Equal(HttpStatusCode.InternalServerError, exception.Status);
        Assert.Equal("Erro interno", exception.Title);
        Assert.Equal("Erro ao gerar certificado.", exception.Detail);
    }

    [Fact]
    public async Task ValidateAsync_ShouldThrowNotImplementedBaseException()
    {
        var (service, _, _, _, _, _) = BuildServiceWithDependencies();

        var exception = await Assert.ThrowsAsync<BaseException>(() => service.ValidateAsync("auth-123"));

        Assert.Equal(HttpStatusCode.NotImplemented, exception.Status);
        Assert.Equal("Valiar Certificado", exception.Title);
        Assert.Equal("Funcionalidade não implementada.", exception.Detail);
    }

    private static void EnsureTemplateFile()
    {
        var templatesPath = Path.Combine(AppContext.BaseDirectory, "Templates");
        Directory.CreateDirectory(templatesPath);

        var templatePath = Path.Combine(templatesPath, "default.html");
        if (File.Exists(templatePath))
            return;

        const string html = """
                            <html>
                              <body>
                                <h1>{{UNIVERSITY_NAME}}</h1>
                                <p>{{STUDENT_NAME}}</p>
                                <p>{{COURSE_DEGREE}}</p>
                                <p>{{COURSE_NAME}}</p>
                                <p>{{COURSE_COMPLETATION}}</p>
                                <p>{{SIGNATURE_LOCATION}}</p>
                                <p>{{SIGNATURE_DATE}}</p>
                                <p>{{SIGNATURE_DEERS}}</p>
                                <p>{{SIGNATURE_ADMINISTRATIVE}}</p>
                                <p>{{DIGITAL_AUTHENTICATION}}</p>
                                <p>{{DOCUMENT_TYPE}}</p>
                                <p>{{DOCUMENT_NUMBER}}</p>
                                <p>{{REGISTRATION_NUMBER}}</p>
                                <p>{{CAMPUS_NAME}}</p>
                                <p>{{TOTAL_MEDIA}}</p>
                                <p>{{TOTAL_HOURS}}</p>
                                <p>{{COURSE_STATUS}}</p>
                                <img style="display:''QRCODE_STYLE''" src="data:image/png;base64,{{BASE64_QRCODE}}" />
                                <table>{{DISCIPLINES_TABLE}}</table>
                                {{TEXT_INFO_FINAL_AVARAGE}}
                                {{NEW_PAGE_N}}
                              </body>
                            </html>
                            """;

        File.WriteAllText(templatePath, html);
    }

    private static void ConfigureValidDependencies(
        CertificateRequest request,
        ICourseService courseService,
        IDisciplineService disciplineService,
        IInstitutionService institutionService)
    {
        institutionService.GetInstitutionAsync(request.InstitutionId).Returns(new InstitutionResponse
        {
            InstitutionId = request.InstitutionId,
            Name = "Universidade Exemplo",
            Acronym = "UEX",
            City = "São Paulo",
            State = "SP",
            Type = "Universidade"
        });

        courseService.GetCourseAsync(request.Course.CourseId).Returns(new CourseResponse
        {
            CourseId = request.Course.CourseId,
            Name = "Engenharia de Software",
            Degree = "Bacharelado",
            TotalWorkload = 3200,
            AverageApproval = 7.0m
        });

        foreach (var discipline in request.Course.Disciplines.DistinctBy(x => x.DisciplineId))
        {
            disciplineService.GetDisciplineAsync(discipline.DisciplineId).Returns(new DisciplineResponse
            {
                DisciplineId = discipline.DisciplineId,
                Name = $"Disciplina {discipline.DisciplineId}",
                Description = "Descrição",
                Types = ["Tecnologia"],
                Workload = 80,
                AverageApproval = 7.0m
            });
        }
    }

    private static (CertificateService service, IPdfService pdfService, ICourseService courseService, IDisciplineService disciplineService, IInstitutionService institutionService, IHttpContextAccessor accessor) BuildServiceWithDependencies()
    {
        var pdfService = Substitute.For<IPdfService>();
        var courseService = Substitute.For<ICourseService>();
        var disciplineService = Substitute.For<IDisciplineService>();
        var institutionService = Substitute.For<IInstitutionService>();
        var accessor = Substitute.For<IHttpContextAccessor>();
        var context = new DefaultHttpContext();
        context.Request.Scheme = "https";
        context.Request.Host = new HostString("localhost:7244");
        accessor.HttpContext.Returns(context);

        var service = new CertificateService(pdfService, courseService, disciplineService, institutionService, accessor);
        return (service, pdfService, courseService, disciplineService, institutionService, accessor);
    }

}
