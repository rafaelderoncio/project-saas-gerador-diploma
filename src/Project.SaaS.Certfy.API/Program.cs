using Microsoft.AspNetCore.Mvc;
using Project.SaaS.Certfy.Core.Handlers;
using Project.SaaS.Certfy.Core.Repositories;
using Project.SaaS.Certfy.Core.Repositories.Interfaces;
using Project.SaaS.Certfy.Core.Services;
using Project.SaaS.Certfy.Core.Services.Interfaces;
using Project.SaaS.Certfy.Domain.Requests;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();

builder.Services.AddSwaggerGen(opts =>
{
    opts.SwaggerDoc("v1", new()
    {
        Title = "CertfyAPI",
        Version = "v1",
        Description = "API de emissão de certificados."
    });
});

builder.Services.AddSingleton<IPdfService, PuppeteerPdfService>();
builder.Services.AddSingleton<ICertificateService, CertificateService>();
builder.Services.AddSingleton<ICourseService, CourseService>();
builder.Services.AddSingleton<IDisciplineService, DisciplineService>();
builder.Services.AddSingleton<IInstitutionService, InstitutionService>();

builder.Services.AddTransient<ICourseRepository, CourseInMemoryRepository>();
builder.Services.AddTransient<IDisciplineRepository, DisciplineInMemoryRepository>();
builder.Services.AddTransient<IInstitutionRepository, InstitutionInMemoryRepository>();

builder.Services.AddHttpContextAccessor();

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
    pattern: "/courses/{courseId}", 
    handler: async (ICourseService service, [FromRoute] string courseId) 
        => Results.Ok(await service.GetCourseAsync(courseId))
).WithTags("Course");

app.MapGet(
    pattern: "/courses", 
    handler: async (ICourseService service, [FromQuery] int page, [FromQuery] int size) 
        => Results.Ok(await service.GetCoursesAsync(new (page, size))) 
).WithTags("Course");

app.MapGet(
    pattern: "/disciplines/{disciplineId}", 
    handler: async (IDisciplineService service, [FromRoute] string disciplineId) 
        => Results.Ok(await service.GetDisciplineAsync(disciplineId))
).WithTags("Discipline");

app.MapGet(
    pattern: "/disciplines", 
    handler: async (IDisciplineService service, [FromQuery] int page, [FromQuery] int size) 
        => Results.Ok(await service.GetDisciplinesAsync(new (page, size)))
).WithTags("Discipline");

app.MapGet(
    pattern: "institutions/{institutionId}", 
    handler: async (IInstitutionService service, [FromRoute] string institutionId) 
        => Results.Ok(await service.GetInstitutionAsync(institutionId))
).WithTags("Institution");

app.MapGet(
    pattern: "/institutions", 
    handler: async (IInstitutionService service, [FromQuery] int page, [FromQuery] int size) 
        => Results.Ok(await service.GetInstitutionsAsync(new (page, size)))
).WithTags("Institution");

app.MapPost(
    pattern: "/certificate/generate", 
    handler: async (ICertificateService service, [FromBody] CertificateRequest request)
        => Results.File(await service.GenerateAsync(request), "application/pdf", $"certificate{request.Student.Registration}.pdf")
).WithTags("Certificate");

app.MapGet(
    pattern: "/certificate/validate", 
    handler: async ([FromQuery] string authentication) 
        => Results.Ok("in proccess")
).WithTags("Certificate");

app.Run();
