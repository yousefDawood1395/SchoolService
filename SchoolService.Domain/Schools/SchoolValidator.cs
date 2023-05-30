using FluentValidation;

namespace SchoolService.Domain.Schools;

class SchoolValidator : AbstractValidator<School>
{
    public SchoolValidator()
    {
        RuleFor(x => x.Name).NotEmpty().MinimumLength(3).MaximumLength(100);
    }
}