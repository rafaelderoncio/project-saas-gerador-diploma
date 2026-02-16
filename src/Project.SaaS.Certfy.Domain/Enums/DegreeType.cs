using System.ComponentModel.DataAnnotations;

namespace Project.SaaS.Certfy.Domain.Enums;

public enum DegreeType
{
    [Display(Name = "Bacharelado")]
    BachelorsDegree, 

    [Display(Name = "Licenciatura")]
    LicentiateDegree, 
    
    [Display(Name = "Tecnólogo")]
    TechnologistDegree
}
