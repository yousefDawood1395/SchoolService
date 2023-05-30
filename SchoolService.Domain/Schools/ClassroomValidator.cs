using FluentValidation;

namespace SchoolService.Domain.Schools;
class ClassroomValidator : AbstractValidator<ClassRoom>
{
    public ClassroomValidator()
    {
        RuleFor(x => x.SchoolId).GreaterThan(0);
        RuleFor(x => x.Name).NotEmpty().MinimumLength(3).MaximumLength(50);
    }
}