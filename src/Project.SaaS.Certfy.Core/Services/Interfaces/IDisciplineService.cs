using System;
using Project.SaaS.Certfy.Domain.Requests;
using Project.SaaS.Certfy.Domain.Responses;

namespace Project.SaaS.Certfy.Core.Services.Interfaces;

/// <summary>
/// Contrato para consulta de disciplinas.
/// </summary>
public interface IDisciplineService
{
    /// <summary>
    /// Obtém uma disciplina específica por identificador.
    /// </summary>
    /// <param name="disciplineId">Identificador da disciplina.</param>
    /// <returns>Dados da disciplina.</returns>
    Task<DisciplineResponse> GetDisciplineAsync(string disciplineId);

    /// <summary>
    /// Lista disciplinas paginadas.
    /// </summary>
    /// <param name="request">Parâmetros de paginação.</param>
    /// <returns>Lista de disciplinas.</returns>
    Task<List<DisciplineResponse>> GetDisciplinesAsync(PaginationRequest request);
}
