using FluentValidation;

namespace SchoolsService.Domain.Tests.ValidatorDummyObject;
public class DummyObjectValidator : AbstractValidator<DummyObject>
{
    public DummyObjectValidator()
    {
        RuleFor(x => x.Id).NotNull().GreaterThan(0).LessThan(100);
        RuleFor(x => x.Name).NotEmpty().NotNull().MinimumLength(3).MaximumLength(10);
        RuleFor(x => x.Date).NotNull().GreaterThan(new DateTime(2000, 1, 1)).LessThan(new DateTime(2020, 1, 1));
        When(x => x.Id == 10, () =>
        {
            RuleFor(x => x.Name).Must(x => x == "Ten");
        });
    }
}
