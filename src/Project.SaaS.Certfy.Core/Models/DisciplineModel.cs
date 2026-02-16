namespace Project.SaaS.Certfy.Core.Models;

/// <summary>
/// Entidade de disciplina utilizada internamente pela camada Core.
/// </summary>
public class DisciplineModel
{
    /// <summary>
    /// Identificador da disciplina.
    /// </summary>
    public string Id { get; set; } = Guid.NewGuid().ToString();

    /// <summary>
    /// Nome da disciplina.
    /// </summary>
    public string Name { get; set; } = default!;

    /// <summary>
    /// Descrição da disciplina.
    /// </summary>
    public string Description { get; set; } = default!;

    /// <summary>
    /// Tipos/categorias associados à disciplina.
    /// </summary>
    public string[] Types { get; set; } = default!;

    /// <summary>
    /// Carga horária da disciplina.
    /// </summary>
    public int Workload { get; set; }

    /// <summary>
    /// Média mínima para aprovação.
    /// </summary>
    public decimal AverageApproval { get; set; }
}
