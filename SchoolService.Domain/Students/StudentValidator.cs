using FluentValidation;

namespace SchoolService.Domain.Students;

class StudentValidator : AbstractValidator<Student>
{
    public StudentValidator()
    {
        RuleFor(x => x.Name).NotEmpty().NotNull().MinimumLength(3).MaximumLength(100);
        When(x => x.SchoolId != null,
            () => RuleFor(x => x.ClassRoomId).NotNull().GreaterThan(0));
        When(x => x.ClassRoomId != null,
            () => RuleFor(x => x.SchoolId).NotNull().GreaterThan(0));
    }
}