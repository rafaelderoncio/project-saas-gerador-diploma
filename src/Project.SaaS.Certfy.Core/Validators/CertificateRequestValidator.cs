using FluentValidation;
using Project.SaaS.Certfy.Domain.Requests;

namespace Project.SaaS.Certfy.Core.Validators;

public class CertificateRequestValidator : AbstractValidator<CertificateRequest>
{
    public CertificateRequestValidator()
    {
        RuleFor(x => x.Student)
            .NotNull().WithMessage("Student é obrigatório")
            .SetValidator(new StudentCertificateRequestValidator());

        RuleFor(x => x.InstitutionId)
            .NotEmpty().WithMessage("InstitutionId é obrigatório")
            .Length(3, 36).WithMessage("InstitutionId deve ter entre 3 e 36 caracteres");

        RuleFor(x => x.ConclusionDate)
            .NotEmpty().WithMessage("ConclusionDate é obrigatório");

        RuleFor(x => x.Course)
            .NotNull().WithMessage("Course é obrigatório")
            .SetValidator(new CourseCertificateRequestValidator());

        RuleFor(x => x.Signature)
            .NotNull().WithMessage("Signature é obrigatório")
            .SetValidator(new SignatureCertificateRequestValidator());
    }
}