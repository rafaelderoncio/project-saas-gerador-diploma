namespace Project.SaaS.Certfy.Core.Services.Interfaces;

public interface IPdfService
{
    Task<byte[]> GenerateAsync(string template);
}
