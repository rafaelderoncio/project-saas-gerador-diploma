
using Project.SaaS.Certfy.Domain.Requests;

namespace Project.SaaS.Certfy.Core.Services.Interfaces;

/// <summary>
/// Contrato para operações de emissão e validação de certificados.
/// </summary>
public interface ICertificateService
{
    /// <summary>
    /// Gera o certificado em PDF a partir dos dados informados.
    /// </summary>
    /// <param name="request">Dados para compor o certificado.</param>
    /// <returns>Arquivo PDF em bytes.</returns>
    Task<byte[]> GenerateAsync(CertificateRequest request);

    /// <summary>
    /// Valida um certificado pelo código de autenticação.
    /// </summary>
    /// <param name="authentication">Hash/código de autenticação do documento.</param>
    /// <returns>Objeto com resultado da validação.</returns>
    Task<object> ValidateAsync(string authentication);
}
