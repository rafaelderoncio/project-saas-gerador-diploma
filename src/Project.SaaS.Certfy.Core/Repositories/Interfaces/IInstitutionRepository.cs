using Project.SaaS.Certfy.Core.Models;

namespace Project.SaaS.Certfy.Core.Repositories.Interfaces;

/// <summary>
/// Contrato de acesso a dados de instituições.
/// </summary>
public interface IInstitutionRepository
{
    /// <summary>
    /// Lista instituições de forma paginada.
    /// </summary>
    /// <param name="size">Quantidade de itens por página.</param>
    /// <param name="page">Página desejada.</param>
    /// <param name="type">Filtro opcional por tipo.</param>
    /// <returns>Coleção de instituições.</returns>
    Task<IEnumerable<InstitutionModel>> GetInstitutionsAsync(int size = 10, int page = 1, string? type = null);

    /// <summary>
    /// Obtém uma instituição por identificador.
    /// </summary>
    /// <param name="institutionId">Identificador da instituição.</param>
    /// <returns>Instituição encontrada.</returns>
    Task<InstitutionModel> GetInstitutionAsync(string institutionId);
}
