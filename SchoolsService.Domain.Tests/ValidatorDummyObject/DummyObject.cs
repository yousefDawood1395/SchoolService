using FluentValidation;
using SchoolService.Domain.Shared.Validation;
namespace SchoolsService.Domain.Tests.ValidatorDummyObject;
public class DummyObject : IValidationModel<DummyObject>
{
    public int? Id { get; set; }
    public string? Name { get; set; }
    public DateTime? Date { get; set; }

    public AbstractValidator<DummyObject> Validator => new DummyObjectValidator();
}
