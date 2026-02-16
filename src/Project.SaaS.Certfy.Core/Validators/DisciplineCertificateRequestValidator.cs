using FluentValidation;
using Project.SaaS.Certfy.Domain.Enums;
using Project.SaaS.Certfy.Domain.Requests;
using Project.SaaS.Certfy.Core.Extensions;

namespace Project.SaaS.Certfy.Core.Validators;

public class DisciplineCertificateRequestValidator 
    : AbstractValidator<DisciplineCertificateRequest>
{
    public DisciplineCertificateRequestValidator()
    {
        RuleFor(x => x.DisciplineId)
            .NotEmpty().WithMessage("Discipline.DisciplineId é obrigatório")
            .Length(3, 36);

        RuleFor(x => x.Period)
            .NotEmpty().WithMessage("Discipline.Period é obrigatório")
            .Matches(@"^\d{4}\/[12]$").WithMessage("Discipline.Period inválido. Formato aceito: AAAA/1 ou AAAA/2");

        RuleFor(x => x.Average)
            .NotEmpty().WithMessage("Discipline.Average é obrigatório")
            .InclusiveBetween(0, 10).WithMessage("Discipline.Average deve estar entre 0 e 10");
    }
}