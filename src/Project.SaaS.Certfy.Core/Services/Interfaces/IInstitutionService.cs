using System;
using Project.SaaS.Certfy.Domain.Requests;
using Project.SaaS.Certfy.Domain.Responses;

namespace Project.SaaS.Certfy.Core.Services.Interfaces;

/// <summary>
/// Contrato para consulta de instituições.
/// </summary>
public interface IInstitutionService
{
    /// <summary>
    /// Obtém uma instituição por identificador.
    /// </summary>
    /// <param name="institutionId">Identificador da instituição.</param>
    /// <returns>Dados da instituição.</returns>
    Task<InstitutionResponse> GetInstitutionAsync(string institutionId);

    /// <summary>
    /// Lista instituições paginadas.
    /// </summary>
    /// <param name="request">Parâmetros de paginação.</param>
    /// <returns>Lista de instituições.</returns>
    Task<List<InstitutionResponse>> GetInstitutionsAsync(PaginationRequest request);
}
