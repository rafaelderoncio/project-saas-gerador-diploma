using System.ComponentModel.DataAnnotations;

namespace Project.SaaS.Certfy.Domain.Enums;

/// <summary>
/// Classificações de disciplinas.
/// </summary>
public enum DisciplineType
{
    /// <summary>
    /// Área de tecnologia.
    /// </summary>
    [Display(Name = "Tecnologia")]
    Technology,

    /// <summary>
    /// Área de dados e analytics.
    /// </summary>
    [Display(Name = "Dados")]
    Data,

    /// <summary>
    /// Área de administração.
    /// </summary>
    [Display(Name = "Administração")]
    Administration,

    /// <summary>
    /// Área de negócios.
    /// </summary>
    [Display(Name = "Negócios")]
    Business,

    /// <summary>
    /// Área do direito.
    /// </summary>
    [Display(Name = "Direito")]
    Law,

    /// <summary>
    /// Área da saúde.
    /// </summary>
    [Display(Name = "Saúde")]
    Health,

    /// <summary>
    /// Área da educação.
    /// </summary>
    [Display(Name = "Educação")]
    Education,

    /// <summary>
    /// Área de engenharia.
    /// </summary>
    [Display(Name = "Engenharia")]
    Engineering,
}
