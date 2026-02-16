using Project.SaaS.Certfy.Core.Models;

namespace Project.SaaS.Certfy.Core.Repositories.Interfaces;

/// <summary>
/// Contrato de acesso a dados de cursos.
/// </summary>
public interface ICourseRepository
{
    /// <summary>
    /// Lista cursos de forma paginada.
    /// </summary>
    /// <param name="size">Quantidade de itens por página.</param>
    /// <param name="page">Página desejada.</param>
    /// <returns>Coleção de cursos.</returns>
    Task<IEnumerable<CourseModel>> GetCoursesAsync(int size = 10, int page = 1);

    /// <summary>
    /// Obtém um curso por identificador.
    /// </summary>
    /// <param name="courseId">Identificador do curso.</param>
    /// <returns>Curso encontrado ou nulo.</returns>
    Task<CourseModel?> GetCourseAsync(string courseId);
}
