using FluentValidation;
using Project.SaaS.Certfy.Core.Extensions;
using Project.SaaS.Certfy.Domain.Enums;
using Project.SaaS.Certfy.Domain.Requests;

namespace Project.SaaS.Certfy.Core.Validators;

public class StudentCertificateRequestValidator : AbstractValidator<StudentCertificateRequest>
{
    public StudentCertificateRequestValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Student.Name é obrigatório")
            .Length(3, 80).WithMessage("Student.Name deve ter entre 3 e 80 caracteres");

        RuleFor(x => x.DocumentNumber)
            .NotEmpty().WithMessage("Student.DocumentNumber é obrigatório")
            .Length(5, 30).WithMessage("Student.DocumentNumber deve ter entre 5 e 30 caracteres");

        RuleFor(x => x.DocumentType)
            .NotEmpty().WithMessage("Student.DocumentType é obrigatório")
            .Must(value => EnumExtensions.GetDisplayNames<DocumentType>().Any(y => y.Equals(value, StringComparison.OrdinalIgnoreCase)))
            .WithMessage(x => $"Student.DocumentType deve ser um dos valores: {string.Join(", ", EnumExtensions.GetDisplayNames<DocumentType>())}");

        RuleFor(x => x.Registration)
            .NotEmpty().WithMessage("Student.Registration é obrigatória")
            .MaximumLength(10).WithMessage("Student.Registration deve conter no máximo 10 caracteres.")
            .Matches(@"^\d+$").WithMessage("Student.Registration deve conter apenas números.");
    }
}