namespace Project.SaaS.Certfy.Domain.Responses;

/// <summary>
/// Retorno com dados da instituição.
/// </summary>
public record InstitutionResponse
{
    /// <summary>
    /// Identificador único da instituição.
    /// </summary>
    public string InstitutionId { get; set; } = default!;

    /// <summary>
    /// Nome completo da instituição.
    /// </summary>
    public string Name { get; set; } = default!;

    /// <summary>
    /// Sigla da instituição.
    /// </summary>
    public string Acronym { get; set; } = default!;

    /// <summary>
    /// Cidade da instituição.
    /// </summary>
    public string City { get; set; } = default!;

    /// <summary>
    /// Estado (UF) da instituição.
    /// </summary>
    public string State { get; set; } = default!;

    /// <summary>
    /// Tipo institucional (ex.: Universidade, Faculdade).
    /// </summary>
    public string Type { get; set; } = default!;
}
