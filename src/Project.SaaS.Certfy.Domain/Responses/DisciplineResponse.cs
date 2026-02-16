namespace Project.SaaS.Certfy.Domain.Responses;

/// <summary>
/// Retorno com dados de disciplina.
/// </summary>
public record DisciplineResponse
{
    /// <summary>
    /// Identificador único da disciplina.
    /// </summary>
    public string DisciplineId { get; set; } = default!;

    /// <summary>
    /// Nome da disciplina.
    /// </summary>
    public string Name { get; set; } = default!;

    /// <summary>
    /// Descrição resumida da disciplina.
    /// </summary>
    public string Description { get; set; } = default!;

    /// <summary>
    /// Categorias/tipos associados à disciplina.
    /// </summary>
    public string[] Types { get; set; } = default!;

    /// <summary>
    /// Carga horária da disciplina.
    /// </summary>
    public int Workload { get; set; }

    /// <summary>
    /// Média mínima para aprovação na disciplina.
    /// </summary>
    public decimal AverageApproval { get; set; }
}
