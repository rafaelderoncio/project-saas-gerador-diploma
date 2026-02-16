using Project.SaaS.Certfy.Core.Models;

namespace Project.SaaS.Certfy.Core.Repositories.Interfaces;

public interface IInstitutionRepository
{
    Task<IEnumerable<InstitutionModel>> GetInstitutionsAsync(int size = 10, int page = 1, string? type = null);
    Task<InstitutionModel> GetInstitutionAsync(string institutionId);
}
