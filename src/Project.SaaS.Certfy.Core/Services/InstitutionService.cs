using System.Net;
using Project.SaaS.Certfy.Core.Exceptions;
using Project.SaaS.Certfy.Core.Repositories.Interfaces;
using Project.SaaS.Certfy.Core.Services.Interfaces;
using Project.SaaS.Certfy.Domain.Requests;
using Project.SaaS.Certfy.Domain.Responses;

namespace Project.SaaS.Certfy.Core.Services;

public class InstitutionService(IInstitutionRepository repository) : IInstitutionService
{
    public async Task<InstitutionResponse> GetInstitutionAsync(string institutionId)
    {
        var institution = await repository.GetInstitutionAsync(institutionId) ??
            throw new BaseException(
                detail: $"Instituição com o ID: {institutionId} não localizada.",
                title: "Erro Disciplina",
                status: HttpStatusCode.NotFound
            );
            
        return new InstitutionResponse
        {
            InstitutionId = institution.Id,
            Name = institution.Name,
            Acronym = institution.Acronym,
            City = institution.City,
            State = institution.State,
            Type = institution.Type
        };
    }

    public async Task<List<InstitutionResponse>> GetInstitutionsAsync(PaginationRequest request)
    {
        request.Size = request.Size <= 0 ? 10 : request.Size;
        request.Page = request.Page <= 0 ? 1 : request.Page;

        var institutions = await repository.GetInstitutionsAsync(request.Size, request.Page);
        return [.. institutions.Select(institution => new InstitutionResponse
        {
            InstitutionId = institution.Id,
            Name = institution.Name,
            Acronym = institution.Acronym,
            City = institution.City,
            State = institution.State,
            Type = institution.Type
        })];
    }
}
