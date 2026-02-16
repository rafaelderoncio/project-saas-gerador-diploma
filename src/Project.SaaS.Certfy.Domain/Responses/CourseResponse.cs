namespace Project.SaaS.Certfy.Domain.Responses;

/// <summary>
/// Retorno com dados de curso.
/// </summary>
public record CourseResponse
{
    /// <summary>
    /// Identificador único do curso.
    /// </summary>
    public string CourseId { get; set; } = default!;

    /// <summary>
    /// Nome do curso.
    /// </summary>
    public string Name { get; set; } = default!;

    /// <summary>
    /// Grau acadêmico do curso (ex.: Bacharelado).
    /// </summary>
    public string Degree { get; set; } = default!;

    /// <summary>
    /// Carga horária total do curso.
    /// </summary>
    public int TotalWorkload { get; set; }

    /// <summary>
    /// Média mínima exigida para aprovação.
    /// </summary>
    public decimal AverageApproval { get; set; }
}
