using Project.SaaS.Certfy.Core.Models;
using Project.SaaS.Certfy.Core.Repositories.Interfaces;

namespace Project.SaaS.Certfy.Core.Repositories;

public class CourseInMemoryRepository : BaseInMemoryRepository<CourseModel>, ICourseRepository
{
    public CourseInMemoryRepository() => LoadAsync();

    public Task<CourseModel?> GetCourseAsync(string courseId)
    {
        var course = _data.FirstOrDefault(
            course => course.Id.ToString().Equals(courseId)
        );
        
        return Task.FromResult(course);
    }

    public Task<IEnumerable<CourseModel>> GetCoursesAsync(int size = 10, int page = 1)
    {
        var skip = (page - 1) * size;
        var courses = _data.Skip(skip).Take(size);

        return Task.FromResult(courses);
    }
}
