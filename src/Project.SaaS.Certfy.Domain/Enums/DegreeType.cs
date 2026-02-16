using System.ComponentModel.DataAnnotations;

namespace Project.SaaS.Certfy.Domain.Enums;

/// <summary>
/// Graus acadêmicos suportados pela plataforma.
/// </summary>
public enum DegreeType
{
    /// <summary>
    /// Curso de Bacharelado.
    /// </summary>
    [Display(Name = "Bacharelado")]
    BachelorsDegree, 

    /// <summary>
    /// Curso de Licenciatura.
    /// </summary>
    [Display(Name = "Licenciatura")]
    LicentiateDegree, 
    
    /// <summary>
    /// Curso Superior de Tecnologia.
    /// </summary>
    [Display(Name = "Tecnólogo")]
    TechnologistDegree
}
