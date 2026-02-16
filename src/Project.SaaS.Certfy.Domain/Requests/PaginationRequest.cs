namespace Project.SaaS.Certfy.Domain.Requests;

/// <summary>
/// Parâmetros de paginação padronizados para listagens.
/// </summary>
public class PaginationRequest(int page = 1, int size = 10)
{
    /// <summary>
    /// Número da página (mínimo 1).
    /// </summary>
    public int Page { get; set; } = page <= 0 ? 1 : page;

    /// <summary>
    /// Quantidade de itens por página (mínimo 1).
    /// </summary>
    public int Size { get; set; } = size <= 0 ? 10 : size;
}
