using System.Net;
using Project.SaaS.Certfy.Core.Exceptions;
using Project.SaaS.Certfy.Core.Repositories.Interfaces;
using Project.SaaS.Certfy.Core.Services.Interfaces;
using Project.SaaS.Certfy.Domain.Requests;
using Project.SaaS.Certfy.Domain.Responses;

namespace Project.SaaS.Certfy.Core.Services;

public class CourseService(ICourseRepository repository) : ICourseService
{
    public async Task<CourseResponse> GetCourseAsync(string courseId)
    {
        var course = await repository.GetCourseAsync(courseId) ??
            throw new BaseException(
                detail: $"Curso com o ID: {courseId} não localizado.",
                title: "Erro Curso",
                status: HttpStatusCode.NotFound
            );

        return new CourseResponse
        {
            CourseId = course.Id,
            Name = course.Name,
            Degree = course.Degree,
            AverageApproval = course.AverageApproval,
            TotalWorkload = course.TotalWorkload
        };
    }

    public async Task<List<CourseResponse>> GetCoursesAsync(PaginationRequest request)
    {
        request.Size = request.Size <= 0 ? 10 : request.Size;
        request.Page = request.Page <= 0 ? 1 : request.Page;

        var courses = await repository.GetCoursesAsync(request.Size, request.Page);
        return [.. courses.Select(course => new CourseResponse
        {
            CourseId = course.Id,
            Name = course.Name,
            Degree = course.Degree,
            AverageApproval = course.AverageApproval,
            TotalWorkload = course.TotalWorkload
        })];
    }
}
