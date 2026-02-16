namespace Project.SaaS.Certfy.Core.Services.Interfaces;

/// <summary>
/// Contrato de geração de PDF a partir de template HTML.
/// </summary>
public interface IPdfService
{
    /// <summary>
    /// Gera um documento PDF com base no HTML final renderizado.
    /// </summary>
    /// <param name="template">HTML completo do documento.</param>
    /// <returns>Conteúdo binário do PDF.</returns>
    Task<byte[]> GenerateAsync(string template);
}
