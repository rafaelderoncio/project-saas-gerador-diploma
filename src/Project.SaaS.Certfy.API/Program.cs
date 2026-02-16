using Microsoft.AspNetCore.Mvc;
using Project.SaaS.Certfy.Core.Handlers;
using Project.SaaS.Certfy.Core.Repositories;
using Project.SaaS.Certfy.Core.Repositories.Interfaces;
using Project.SaaS.Certfy.Core.Services;
using Project.SaaS.Certfy.Core.Services.Interfaces;
using Project.SaaS.Certfy.Domain.Requests;
using Project.SaaS.Certfy.Domain.Responses;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddEndpointsApiExplorer()
    .AddHttpContextAccessor()
    .AddExceptionHandler<GlobalExceptionHandler>()
    .AddProblemDetails();

builder.Services
    .AddSingleton<IPdfService, PuppeteerPdfService>()
    .AddSingleton<ICertificateService, CertificateService>()
    .AddSingleton<ICourseService, CourseService>()
    .AddSingleton<IDisciplineService, DisciplineService>()
    .AddSingleton<IInstitutionService, InstitutionService>();

builder.Services
    .AddTransient<ICourseRepository, CourseInMemoryRepository>()
    .AddTransient<IDisciplineRepository, DisciplineInMemoryRepository>()
    .AddTransient<IInstitutionRepository, InstitutionInMemoryRepository>();

builder.Services.AddSwaggerGen(opts =>
{
    opts.SwaggerDoc("v1", new()
    {
        Title = "CertfyAPI",
        Version = "v1",
        Description = "API de emissão e validação de certificados acadêmicos."
    });

    foreach (var xmlPath in Directory.EnumerateFiles(AppContext.BaseDirectory, "*.xml", SearchOption.TopDirectoryOnly))
        opts.IncludeXmlComments(xmlPath, includeControllerXmlComments: true);
});

var app = builder.Build();

if (!app.Environment.IsProduction())
    app.UseSwagger().UseSwaggerUI(opts =>
    {
        opts.SwaggerEndpoint("/swagger/v1/swagger.json", "Certfy API - v1");
        opts.RoutePrefix = "swagger";
    });

app.UseHttpsRedirection();

app.UseHsts();

app.UseExceptionHandler();

app.MapGet(
    pattern: "api/courses/{courseId}", 
    handler: async (ICourseService service, [FromRoute] string courseId) 
        => Results.Ok(await service.GetCourseAsync(courseId))
)
.WithTags("Course")
.WithName("GetCourseById")
.WithSummary("Obtém curso por ID.")
.WithDescription("Retorna os dados de um curso específico.")
.Produces<CourseResponse>(StatusCodes.Status200OK)
.Produces<ProblemDetails>(StatusCodes.Status404NotFound)
.ProducesProblem(StatusCodes.Status500InternalServerError);

app.MapGet(
    pattern: "api/courses", 
    handler: async (ICourseService service, [FromQuery] int page, [FromQuery] int size) 
        => Results.Ok(await service.GetCoursesAsync(new (page, size))) 
)
.WithTags("Course")
.WithName("ListCourses")
.WithSummary("Lista cursos paginados.")
.WithDescription("Retorna cursos com base nos parâmetros de paginação.")
.Produces<IEnumerable<CourseResponse>>(StatusCodes.Status200OK)
.ProducesProblem(StatusCodes.Status500InternalServerError);

app.MapGet(
    pattern: "api/disciplines/{disciplineId}", 
    handler: async (IDisciplineService service, [FromRoute] string disciplineId) 
        => Results.Ok(await service.GetDisciplineAsync(disciplineId))
)
.WithTags("Discipline")
.WithName("GetDisciplineById")
.WithSummary("Obtém disciplina por ID.")
.WithDescription("Retorna os dados de uma disciplina específica.")
.Produces<DisciplineResponse>(StatusCodes.Status200OK)
.Produces<ProblemDetails>(StatusCodes.Status404NotFound)
.ProducesProblem(StatusCodes.Status500InternalServerError);

app.MapGet(
    pattern: "api/disciplines", 
    handler: async (IDisciplineService service, [FromQuery] int page, [FromQuery] int size) 
        => Results.Ok(await service.GetDisciplinesAsync(new (page, size)))
)
.WithTags("Discipline")
.WithName("ListDisciplines")
.WithSummary("Lista disciplinas paginadas.")
.WithDescription("Retorna disciplinas com base nos parâmetros de paginação.")
.Produces<IEnumerable<DisciplineResponse>>(StatusCodes.Status200OK)
.ProducesProblem(StatusCodes.Status500InternalServerError);

app.MapGet(
    pattern: "institutions/{institutionId}", 
    handler: async (IInstitutionService service, [FromRoute] string institutionId) 
        => Results.Ok(await service.GetInstitutionAsync(institutionId))
)
.WithTags("Institution")
.WithName("GetInstitutionById")
.WithSummary("Obtém instituição por ID.")
.WithDescription("Retorna os dados de uma instituição específica.")
.Produces<InstitutionResponse>(StatusCodes.Status200OK)
.Produces<ProblemDetails>(StatusCodes.Status404NotFound)
.ProducesProblem(StatusCodes.Status500InternalServerError);

app.MapGet(
    pattern: "api/institutions", 
    handler: async (IInstitutionService service, [FromQuery] int page, [FromQuery] int size) 
        => Results.Ok(await service.GetInstitutionsAsync(new (page, size)))
)
.WithTags("Institution")
.WithName("ListInstitutions")
.WithSummary("Lista instituições paginadas.")
.WithDescription("Retorna instituições com base nos parâmetros de paginação.")
.Produces<IEnumerable<InstitutionResponse>>(StatusCodes.Status200OK)
.ProducesProblem(StatusCodes.Status500InternalServerError);

app.MapPost(
    pattern: "api/certificate/generate", 
    handler: async (ICertificateService service, [FromBody] CertificateRequest request)
        => Results.File(await service.GenerateAsync(request), "application/pdf", $"certificate{request.Student.Registration}.pdf")
)
.WithTags("Certificate")
.WithName("GenerateCertificate")
.WithSummary("Gera certificado em PDF.")
.WithDescription("Recebe os dados acadêmicos e retorna um arquivo PDF de certificado.")
.Accepts<CertificateRequest>("application/json")
.Produces(StatusCodes.Status200OK, contentType: "application/pdf")
.Produces<ProblemDetails>(StatusCodes.Status400BadRequest)
.ProducesProblem(StatusCodes.Status500InternalServerError);

app.MapGet(
    pattern: "api/certificate/validate", 
    handler: async (ICertificateService service, [FromQuery] string authentication) 
        => Results.Ok(await service.ValidateAsync(authentication))
)
.WithTags("Certificate")
.WithName("ValidateCertificate")
.WithSummary("Valida certificado por código.")
.WithDescription("Valida autenticidade de um certificado a partir do código de autenticação.")
.Produces(StatusCodes.Status200OK)
.ProducesProblem(StatusCodes.Status400BadRequest);

app.Run();
