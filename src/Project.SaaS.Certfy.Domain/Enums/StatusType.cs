using System.ComponentModel.DataAnnotations;

namespace Project.SaaS.Certfy.Domain.Enums;

public enum StatusType
{
    [Display(Name = "Aprovado")]
    Appoved,

    [Display(Name = "Reprovado")]
    Repproved,

    [Display(Name = "Dispensado")]
    Dispensed
}
