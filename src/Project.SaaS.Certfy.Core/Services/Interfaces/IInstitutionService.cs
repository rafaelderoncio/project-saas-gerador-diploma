using System;
using Project.SaaS.Certfy.Domain.Requests;
using Project.SaaS.Certfy.Domain.Responses;

namespace Project.SaaS.Certfy.Core.Services.Interfaces;

public interface IInstitutionService
{
    Task<InstitutionResponse> GetInstitutionAsync(string institutionId);
    Task<List<InstitutionResponse>> GetInstitutionsAsync(PaginationRequest request);
}
