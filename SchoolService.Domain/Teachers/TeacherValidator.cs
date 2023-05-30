using FluentValidation;

namespace SchoolService.Domain.Teachers;

class TeacherValidator : AbstractValidator<Teacher>
{
    public TeacherValidator()
    {
        RuleFor(x => x.Name).NotNull().NotEmpty().MinimumLength(3).MaximumLength(100);
    }
}