namespace Project.SaaS.Certfy.Core.Models;

/// <summary>
/// Entidade de instituição utilizada internamente pela camada Core.
/// </summary>
public record class InstitutionModel
{
    /// <summary>
    /// Identificador da instituição.
    /// </summary>
    public string Id { get; set; } = Guid.NewGuid().ToString();

    /// <summary>
    /// Nome da instituição.
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
    /// Estado/UF da instituição.
    /// </summary>
    public string State { get; set; } = default!;

    /// <summary>
    /// Tipo da instituição.
    /// </summary>
    public string Type { get; set; } = default!;
}
