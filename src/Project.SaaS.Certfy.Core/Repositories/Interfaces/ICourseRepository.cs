using Project.SaaS.Certfy.Core.Models;

namespace Project.SaaS.Certfy.Core.Repositories.Interfaces;

public interface ICourseRepository
{
    Task<IEnumerable<CourseModel>> GetCoursesAsync(int size = 10, int page = 1);
    Task<CourseModel?> GetCourseAsync(string courseId);
}
