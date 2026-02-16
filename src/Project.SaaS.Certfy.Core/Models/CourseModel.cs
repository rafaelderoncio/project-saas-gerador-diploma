namespace Project.SaaS.Certfy.Core.Models;

/// <summary>
/// Entidade de curso utilizada internamente pela camada Core.
/// </summary>
public class CourseModel
{
    /// <summary>
    /// Identificador do curso.
    /// </summary>
    public string Id { get; set; } = Guid.NewGuid().ToString();

    /// <summary>
    /// Nome do curso.
    /// </summary>
    public string Name { get; set; } = default!;

    /// <summary>
    /// Grau acadêmico do curso.
    /// </summary>
    public string Degree { get; set; } = default!;

    /// <summary>
    /// Carga horária total do curso.
    /// </summary>
    public int TotalWorkload { get; set; } = 24;

    /// <summary>
    /// Média mínima para aprovação no curso.
    /// </summary>
    public decimal AverageApproval { get; set; } = 7.0m;
}
