using System;
using System.Net;
using Project.SaaS.Certfy.Core.Exceptions;
using Project.SaaS.Certfy.Core.Repositories.Interfaces;
using Project.SaaS.Certfy.Core.Services.Interfaces;
using Project.SaaS.Certfy.Domain.Requests;
using Project.SaaS.Certfy.Domain.Responses;

namespace Project.SaaS.Certfy.Core.Services;

public class DisciplineService(IDisciplineRepository repository) : IDisciplineService
{
    public async Task<DisciplineResponse> GetDisciplineAsync(string disciplineId)
    {
        var discipline = await repository.GetDisciplineAsync(disciplineId) ??
            throw new BaseException(
                detail: $"Disciplina com o ID: {disciplineId} não localizada.",
                title: "Erro Disciplina",
                status: HttpStatusCode.NotFound
            );

        return new DisciplineResponse
        {
            DisciplineId = discipline.Id,
            Name = discipline.Name,
            Description = discipline.Description,
            AverageApproval = discipline.AverageApproval,
            Types = discipline.Types,
            Workload = discipline.Workload
        };
    }

    public async Task<List<DisciplineResponse>> GetDisciplinesAsync(PaginationRequest request)
    {
        request.Size = request.Size <= 0 ? 10 : request.Size;
        request.Page = request.Page <= 0 ? 1 : request.Page;

        var disciplines = await repository.GetDisciplineAsync(request.Size, request.Page);
        return [.. disciplines.Select(discipline => new DisciplineResponse
        {
            DisciplineId = discipline.Id,
            Name = discipline.Name,
            Description = discipline.Description,
            AverageApproval = discipline.AverageApproval,
            Types = discipline.Types,
            Workload = discipline.Workload
        })];
    }
}
