using System.ComponentModel.DataAnnotations;

namespace Project.SaaS.Certfy.Domain.Enums;

/// <summary>
/// Status acadêmico final de disciplina/curso no certificado.
/// </summary>
public enum StatusType
{
    /// <summary>
    /// Aprovado.
    /// </summary>
    [Display(Name = "Aprovado")]
    Appoved,

    /// <summary>
    /// Reprovado.
    /// </summary>
    [Display(Name = "Reprovado")]
    Repproved,

    /// <summary>
    /// Dispensado.
    /// </summary>
    [Display(Name = "Dispensado")]
    Dispensed
}
