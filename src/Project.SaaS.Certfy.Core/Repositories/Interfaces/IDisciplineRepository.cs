using System;
using Project.SaaS.Certfy.Core.Models;

namespace Project.SaaS.Certfy.Core.Repositories.Interfaces;

public interface IDisciplineRepository
{
    Task<IEnumerable<DisciplineModel>> GetDisciplineAsync(int size = 10, int page = 1, string? type = null);
    Task<DisciplineModel> GetDisciplineAsync(string disciplineId);
}
