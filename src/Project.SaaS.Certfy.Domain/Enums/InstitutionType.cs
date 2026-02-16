using System.ComponentModel.DataAnnotations;

namespace Project.SaaS.Certfy.Domain.Enums;

/// <summary>
/// Tipos institucionais suportados.
/// </summary>
public enum InstitutionType
{
    /// <summary>
    /// Universidade.
    /// </summary>
    [Display(Name = "Universidade")]
    University, 
    
    /// <summary>
    /// Faculdade.
    /// </summary>
    [Display(Name = "Faculdade")]
    College, 

    /// <summary>
    /// Instituto.
    /// </summary>
    [Display(Name = "Instituto")]
    Institute, 

    /// <summary>
    /// Centro universitário.
    /// </summary>
    [Display(Name = "Centro Universitário")]
    UniversityCenter
}
