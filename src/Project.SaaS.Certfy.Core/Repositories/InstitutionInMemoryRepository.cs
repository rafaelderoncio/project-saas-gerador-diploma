using Project.SaaS.Certfy.Core.Models;
using Project.SaaS.Certfy.Core.Repositories.Interfaces;

namespace Project.SaaS.Certfy.Core.Repositories;

public class InstitutionInMemoryRepository : BaseInMemoryRepository<InstitutionModel>, IInstitutionRepository
{
    public InstitutionInMemoryRepository() => LoadAsync();

    public Task<IEnumerable<InstitutionModel>> GetInstitutionsAsync(int size = 10, int page = 1, string? type = null)
    {
        var skip = (page - 1) * size;
        var institutions = _data
            .Where(institution =>
                string.IsNullOrWhiteSpace(type) ||
                institution.Type.Equals(type, StringComparison.OrdinalIgnoreCase))
            .Skip(skip)
            .Take(size);

        return Task.FromResult(institutions);
    }

    public Task<InstitutionModel> GetInstitutionAsync(string institutionId)
    {
        var disciplines = _data.FirstOrDefault(
            institution => institution.Id.Equals(institutionId)
        ) ?? throw new KeyNotFoundException($"Institution '{institutionId}' was not found.");
        
        return Task.FromResult(disciplines);
    }
}
