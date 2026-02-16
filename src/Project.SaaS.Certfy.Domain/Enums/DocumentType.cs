using System.ComponentModel.DataAnnotations;

namespace Project.SaaS.Certfy.Domain.Enums;

public enum DocumentType
{
    [Display(Name = "CPF")]
    CPF,

    [Display(Name = "RG")]
    RG,

    [Display(Name = "CNH")]
    CNH,
}
