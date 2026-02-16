using System;
using Project.SaaS.Certfy.Domain.Requests;
using Project.SaaS.Certfy.Domain.Responses;

namespace Project.SaaS.Certfy.Core.Services.Interfaces;

public interface IDisciplineService
{
    Task<DisciplineResponse> GetDisciplineAsync(string disciplineId);
    Task<List<DisciplineResponse>> GetDisciplinesAsync(PaginationRequest request);
}
