using Project.SaaS.Certfy.Domain.Requests;
using Project.SaaS.Certfy.Domain.Responses;

namespace Project.SaaS.Certfy.Core.Services.Interfaces;

/// <summary>
/// Contrato para consulta de cursos.
/// </summary>
public interface ICourseService
{
    /// <summary>
    /// Obtém um curso específico por identificador.
    /// </summary>
    /// <param name="courseId">Identificador do curso.</param>
    /// <returns>Dados do curso.</returns>
    Task<CourseResponse> GetCourseAsync(string courseId);

    /// <summary>
    /// Lista cursos paginados.
    /// </summary>
    /// <param name="request">Parâmetros de paginação.</param>
    /// <returns>Lista de cursos.</returns>
    Task<List<CourseResponse>> GetCoursesAsync(PaginationRequest request);
}
