using System;
using Project.SaaS.Certfy.Core.Models;

namespace Project.SaaS.Certfy.Core.Repositories.Interfaces;

/// <summary>
/// Contrato de acesso a dados de disciplinas.
/// </summary>
public interface IDisciplineRepository
{
    /// <summary>
    /// Lista disciplinas de forma paginada.
    /// </summary>
    /// <param name="size">Quantidade de itens por página.</param>
    /// <param name="page">Página desejada.</param>
    /// <param name="type">Filtro opcional por tipo.</param>
    /// <returns>Coleção de disciplinas.</returns>
    Task<IEnumerable<DisciplineModel>> GetDisciplineAsync(int size = 10, int page = 1, string? type = null);

    /// <summary>
    /// Obtém uma disciplina por identificador.
    /// </summary>
    /// <param name="disciplineId">Identificador da disciplina.</param>
    /// <returns>Disciplina encontrada.</returns>
    Task<DisciplineModel> GetDisciplineAsync(string disciplineId);
}
