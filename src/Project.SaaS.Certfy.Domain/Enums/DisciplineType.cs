using System.ComponentModel.DataAnnotations;

namespace Project.SaaS.Certfy.Domain.Enums;

public enum DisciplineType
{
    [Display(Name = "Tecnologia")]
    Technology,

    [Display(Name = "Dados")]
    Data,

    [Display(Name = "Administração")]
    Administration,

    [Display(Name = "Negócios")]
    Business,

    [Display(Name = "Direito")]
    Law,

    [Display(Name = "Saúde")]
    Health,

    [Display(Name = "Educação")]
    Education,

    [Display(Name = "Engenharia")]
    Engineering,
}
