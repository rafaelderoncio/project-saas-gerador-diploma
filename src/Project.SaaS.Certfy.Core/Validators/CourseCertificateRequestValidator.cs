using FluentValidation;
using Project.SaaS.Certfy.Core.Extensions;
using Project.SaaS.Certfy.Domain.Enums;
using Project.SaaS.Certfy.Domain.Requests;

namespace Project.SaaS.Certfy.Core.Validators;

public class CourseCertificateRequestValidator : AbstractValidator<CourseCertificateRequest>
{
    public CourseCertificateRequestValidator()
    {
        RuleFor(x => x.CourseId)
            .NotEmpty().WithMessage("Course.CourseId é obrigatório")
            .Length(3, 36);

        RuleFor(x => x.Disciplines)
            .NotNull().WithMessage("Course.Disciplines é obrigatória")
            .Must(d => d.Count >= 1).WithMessage("Course.Disciplines Deve existir ao menos uma disciplina");

        RuleForEach(x => x.Disciplines)
            .SetValidator(new DisciplineCertificateRequestValidator());
    }
}
