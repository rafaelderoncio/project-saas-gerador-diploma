using System;
using Project.SaaS.Certfy.Core.Models;
using Project.SaaS.Certfy.Core.Repositories.Interfaces;

namespace Project.SaaS.Certfy.Core.Repositories;

public class DisciplineInMemoryRepository : BaseInMemoryRepository<DisciplineModel>, IDisciplineRepository
{

    public DisciplineInMemoryRepository() => LoadAsync();

    public Task<IEnumerable<DisciplineModel>> GetDisciplineAsync(int size = 10, int page = 1, string? type = null)
    {
        var skip = (page - 1) * size;
        var disciplines = _data
            .Where(discipline =>
                string.IsNullOrWhiteSpace(type) ||
                discipline.Types.Any(disciplineType =>
                    disciplineType.Equals(type, StringComparison.OrdinalIgnoreCase)))
            .Skip(skip)
            .Take(size);

        return Task.FromResult(disciplines);
    }

    public Task<DisciplineModel> GetDisciplineAsync(string disciplineId)
    {
        var disciplines = _data.FirstOrDefault(
            discipline => discipline.Id.ToString().Equals(disciplineId)
        ) ?? throw new KeyNotFoundException($"Discipline '{disciplineId}' was not found.");
        
        return Task.FromResult(disciplines);
    }
}
