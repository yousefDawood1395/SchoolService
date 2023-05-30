using AutoFixture;
using FluentValidation.Results;
using Moq;
using SchoolService.Domain.Shared.Exceptions;
using SchoolService.Domain.Shared.Pagination;
using SchoolService.Domain.Shared.Repositories;
using SchoolService.Domain.Shared.Validation;
using SchoolService.Domain.Teachers;
using System.Security;

namespace SchoolsService.Domain.Tests;
public class TeacherTests
{
    private readonly Mock<ITeachersRepository> _teachersRepository;
    private readonly Mock<IValidationEngine> _validationEngine;
    private readonly Fixture _fixture;
    public TeacherTests()
    {
        _teachersRepository = new Mock<ITeachersRepository>();
        _validationEngine = new Mock<IValidationEngine>();
        _validationEngine.Setup(x => x.Validate(It.IsAny<Teacher>(), true)).Returns(new List<ValidationFailure>());
        _fixture = new Fixture();
    }

    [Fact]
    public async Task Create_ValidInput_Success()
    {
        _teachersRepository.Setup(x => x.Create(It.IsAny<Teacher>())).ReturnsAsync(2);
        var teacher = ValidTeacher(_fixture);
        var output = await teacher.Create(_validationEngine.Object, _teachersRepository.Object);

        Assert.Equal(2, output);
    }

    [Fact]
    public async Task Update_ValidInput_Success()
    {
        var teacher = ValidTeacher(_fixture);
        _teachersRepository.Setup(x => x.Get(It.IsAny<int>())).ReturnsAsync(teacher);
        _teachersRepository.Setup(x => x.Update(It.IsAny<Teacher>())).ReturnsAsync(true);

        var output = await teacher.Update(_validationEngine.Object, _teachersRepository.Object);

        Assert.True(output);
    }

    [Fact]
    public async Task Search_ValidInput_Success()
    {
        var pagedTeachers = PagedTeachers(_fixture);
        _teachersRepository.Setup(x => x.Search(It.IsAny<string?>(), It.IsAny<int?>(), It.IsAny<int?>(), It.IsAny<int>(), It.IsAny<int>()))
            .ReturnsAsync(pagedTeachers);

        var output = await Teacher.Search(_fixture.Create<string>(), _fixture.Create<int>(), _fixture.Create<int>(), _fixture.Create<int>(), _fixture.Create<int>(), _teachersRepository.Object);

        Assert.Equal(pagedTeachers, output);
    }

    [Fact]
    public void SetName_ValidInput_Success()
    {
        var teacher = ValidTeacher(_fixture);
        var newName = _fixture.Create<string>();
        teacher.SetName(newName);

        Assert.Equal(newName, teacher.Name);
    }

    [Theory]
    [InlineData(null, null)]
    [InlineData(0, "name")]
    [InlineData(1, "name")]
    public void CreateTeacher_ValidInput_Success(int? id, string? name)
    {
        var teacher = Teacher.Create(id, name);

        Assert.Equal(id ?? 0, teacher.Id);
        Assert.Equal(name, teacher.Name);
    }

    [Fact]
    public async Task Get_ValidInput_Success()
    {
        var teacher = ValidTeacher(_fixture);

        _teachersRepository.Setup(x => x.Get(It.IsAny<int>())).ReturnsAsync(teacher);

        var output = await Teacher.Get(_fixture.Create<int>(), _teachersRepository.Object);

        Assert.Equal(teacher, output);
    }

    [Fact]
    public async Task Get_InvalidInput_ThrowsNotFountException()
    {
        _teachersRepository.Setup(x => x.Get(It.IsAny<int>())).ReturnsAsync((Teacher?)null);
        await Assert.ThrowsAsync<DataNotFoundException>(() => Teacher.Get(_fixture.Create<int>(), _teachersRepository.Object));
    }

    [Fact]
    public async Task Delete_ValidInput_Success()
    {
        var teacher = ValidTeacher(_fixture);
        _teachersRepository.Setup(x => x.Get(It.IsAny<int>())).ReturnsAsync(teacher);
        _teachersRepository.Setup(x => x.Delete(It.IsAny<Teacher>())).ReturnsAsync(true);

        var output = await Teacher.Delete(_fixture.Create<int>(), _teachersRepository.Object);

        Assert.True(output);
    }

    [Fact]
    public async Task Delete_InvalidInput_ThrowsNotFoundException()
    {
        var teacher = ValidTeacher(_fixture);
        _teachersRepository.Setup(x => x.Get(It.IsAny<int>())).ReturnsAsync((Teacher?)null);
        _teachersRepository.Setup(x => x.Delete(It.IsAny<Teacher>())).ReturnsAsync(true);

        await Assert.ThrowsAsync<DataNotFoundException>(() => Teacher.Delete(_fixture.Create<int>(), _teachersRepository.Object));
    }

    [Fact]
    public async Task Delete_InvalidInput_ThrowsBusinessException()
    {
        var teacher = ValidTeacher(_fixture);
        teacher.Classrooms.Add(ValidClassroom(_fixture));
        _teachersRepository.Setup(x => x.Get(It.IsAny<int>())).ReturnsAsync(teacher);
        _teachersRepository.Setup(x => x.Delete(It.IsAny<Teacher>())).ReturnsAsync(true);

        await Assert.ThrowsAsync<BusinessException>(() => Teacher.Delete(_fixture.Create<int>(), _teachersRepository.Object));
    }


    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("n")]
    [InlineData("12345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890")]
    public void Teacher_Validator_Tests(string? name)
    {
        int id = _fixture.Create<int>();
        var teacher = Teacher.Create(id, name);
        var validation = new ValidationEngine();
        Assert.Throws<DataNotValidException>(() => validation.Validate(teacher));
    }
    private static Teacher ValidTeacher(Fixture fixture) =>
        Teacher.Create(fixture.Create<int>(), fixture.Create<string>());

    private static TeacherClassrooms ValidClassroom(Fixture fixture) =>
        TeacherClassrooms.Create(fixture.Create<int>(), fixture.Create<int>(), fixture.Create<int>(), fixture.Create<int>());

    private static PagedResponse<Teacher> PagedTeachers(Fixture fixture) =>
        new PagedResponse<Teacher>
        {
            PageNumber = 1,
            PageSize = 10,
            TotalCount = 10,
            Data = Enumerable.Range(1, 10).Select(s => ValidTeacher(fixture)).ToList()
        };
}
