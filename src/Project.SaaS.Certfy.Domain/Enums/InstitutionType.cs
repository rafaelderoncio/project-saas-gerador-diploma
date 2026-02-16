using System.ComponentModel.DataAnnotations;

namespace Project.SaaS.Certfy.Domain.Enums;

public enum InstitutionType
{
    [Display(Name = "Universidade")]
    University, 
    
    [Display(Name = "Faculdade")]
    College, 

    [Display(Name = "Instituto")]
    Institute, 

    [Display(Name = "Centro Universitário")]
    UniversityCenter
}
