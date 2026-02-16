
using Project.SaaS.Certfy.Domain.Requests;

namespace Project.SaaS.Certfy.Core.Services.Interfaces;

public interface ICertificateService
{
    Task<byte[]> GenerateAsync(CertificateRequest request);
    Task<object> ValidateAsync(string authentication);
}
