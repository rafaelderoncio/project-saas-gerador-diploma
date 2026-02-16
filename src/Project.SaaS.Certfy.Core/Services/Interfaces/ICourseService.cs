using Project.SaaS.Certfy.Domain.Requests;
using Project.SaaS.Certfy.Domain.Responses;

namespace Project.SaaS.Certfy.Core.Services.Interfaces;

public interface ICourseService
{
    Task<CourseResponse> GetCourseAsync(string courseId);
    Task<List<CourseResponse>> GetCoursesAsync(PaginationRequest request);
}
