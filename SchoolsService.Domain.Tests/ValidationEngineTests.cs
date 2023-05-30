using SchoolService.Domain.Shared.Exceptions;
using SchoolService.Domain.Shared.Validation;
using SchoolsService.Domain.Tests.ValidatorDummyObject;

namespace SchoolsService.Domain.Tests;
public class ValidationEngineTests
{
    private readonly IValidationEngine _validationEngine;

    public ValidationEngineTests()
    {
        _validationEngine = new ValidationEngine();
    }

    [Theory]
    [InlineData(null, null, null, 4)]
    [InlineData(0, null, null, 4)]
    [InlineData(200, null, null, 4)]
    [InlineData(20, null, null, 3)]
    [InlineData(20, "", null, 3)]
    [InlineData(20, "s", null, 2)]
    [InlineData(20, "123456789012345678901234567890", null, 2)]
    [InlineData(20, "name", null, 1)]
    [InlineData(20, "name", "1999-1-1", 1)]
    [InlineData(20, "name", "2021-1-1", 1)]
    public void Validate_InvalidInput_ErrorCountSuccess(int? id, string? name, string? date, int errorCount)
    {
        DateTime? dt = null;
        if (!string.IsNullOrEmpty(date))
        {
            dt = DateTime.Parse(date);
        }

        var validationErrors = _validationEngine.Validate(new DummyObject
        {
            Date = dt,
            Id = id,
            Name = name
        }, false);

        Assert.NotNull(validationErrors);

        Assert.Equal(errorCount, validationErrors.Count());
    }

    [Fact]
    public void Validate_InvalidInput_TestWhen_ThrowsNotValidexception()
    {
        var dummy = new DummyObject
        {
            Id = 10,
            Name = "not ten",
            Date = new DateTime(2010, 1, 1)
        };
        Assert.Throws<DataNotValidException>(() => _validationEngine.Validate(dummy));
    }

    [Fact]
    public void Validate_NullObject_ReturnsNull()
    {
        Assert.Null(_validationEngine.Validate((DummyObject?)null));
    }

    [Fact]
    public void Validate_ValidInput_Success()
    {
        Assert.Null(_validationEngine.Validate(new DummyObject
        {
            Id = 12,
            Date = new DateTime(2012, 12, 12),
            Name = "name"
        }));
    }
}
