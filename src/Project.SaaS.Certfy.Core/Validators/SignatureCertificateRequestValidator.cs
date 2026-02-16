using System;
using FluentValidation;
using Project.SaaS.Certfy.Domain.Requests;

namespace Project.SaaS.Certfy.Core.Validators;

public class SignatureCertificateRequestValidator : AbstractValidator<SignatureCertificateRequest>
{
    public SignatureCertificateRequestValidator()
    {
        RuleFor(x => x.DeersPersonName)
            .NotEmpty().WithMessage("Signature.DeersPersonName é obrigatório")
            .Length(3, 100).WithMessage("Signature.DeersPersonName deve ter entre 3 e 100 caracteres");

        RuleFor(x => x.AdministrativePersonName)
            .NotEmpty().WithMessage("Signature.AdmnistrativePersonName é obrigatório")
            .Length(3, 100).WithMessage("Signature.AdmnistrativePersonName deve ter entre 3 e 100 caracteres");
    }
}