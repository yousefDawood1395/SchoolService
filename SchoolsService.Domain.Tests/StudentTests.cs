using AutoFixture;
using FluentValidation.Results;
using Moq;
using SchoolService.Domain.Shared.Exceptions;
using SchoolService.Domain.Shared.Pagination;
using SchoolService.Domain.Shared.Repositories;
using SchoolService.Domain.Shared.Validation;
using SchoolService.Domain.Students;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolsService.Domain.Tests;
public class StudentTests
{
    private readonly Mock<IStudentsRepository> _studentsRepository;
    private readonly Mock<IValidationEngine> _validationEngine;
    private readonly Fixture _fixture;

    public StudentTests()
    {
        _studentsRepository = new Mock<IStudentsRepository>();
        _validationEngine = new Mock<IValidationEngine>();
        _validationEngine.Setup(x => x.Validate(It.IsAny<Student>(), true)).Returns((List<ValidationFailure>?)null);
        _fixture = new Fixture();
    }

    [Fact]
    public async Task Create_ValidInput_Success()
    {
        _studentsRepository.Setup(x => x.Search(It.IsAny<string?>(), It.IsAny<int?>(), It.IsAny<int?>(), It.IsAny<int>(), It.IsAny<int>())).ReturnsAsync(PagedStudentsEmpty());
        _studentsRepository.Setup(x => x.Create(It.IsAny<Student>())).ReturnsAsync(2);
        var student = ValidStudent(_fixture);
        var output = await student.Create(_studentsRepository.Object, _validationEngine.Object);

        Assert.Equal(2, output);
    }


    [Fact]
    public async Task Update_ValidInput_Success()
    {
        _studentsRepository.Setup(x => x.Search(It.IsAny<string?>(), It.IsAny<int?>(), It.IsAny<int?>(), It.IsAny<int>(), It.IsAny<int>())).ReturnsAsync(PagedStudentsEmpty());
        _studentsRepository.Setup(x => x.Update(It.IsAny<Student>())).ReturnsAsync(true);
        var student = ValidStudent(_fixture);
        var output = await student.Update(_studentsRepository.Object, _validationEngine.Object);

        Assert.True(output);
    }

    [Fact]
    public async Task Delete_ValidInput_Success()
    {
        _studentsRepository.Setup(x => x.Delete(It.IsAny<Student>())).ReturnsAsync(true);
        var student = ValidStudent(_fixture);
        var output = await student.Delete(_studentsRepository.Object);

        Assert.True(output);
    }

    [Fact]
    public async Task Search_ValidInput_Success()
    {
        var pagedStudents = PagedStudents(_fixture);
        _studentsRepository.Setup(x => x.Search(It.IsAny<string?>(), It.IsAny<int?>(), It.IsAny<int?>(), It.IsAny<int>(), It.IsAny<int>())).ReturnsAsync(pagedStudents);

        var output = await Student.Search(_studentsRepository.Object, _fixture.Create<string?>(), _fixture.Create<int?>(), _fixture.Create<int?>(), _fixture.Create<int>(), _fixture.Create<int>());

        Assert.NotNull(output);

        Assert.Equal(pagedStudents, output);
    }


    [Fact]
    public async Task Get_ValidInput_Success()
    {
        var student = ValidStudent(_fixture);
        _studentsRepository.Setup(x => x.Get(It.IsAny<int>())).ReturnsAsync(student);

        var output = await Student.Get(_fixture.Create<int>(), _studentsRepository.Object);

        Assert.NotNull(output);

        Assert.Equal(student, output);
    }

    [Fact]
    public async Task Get_InvalidInput_ThrowsNotFoundException()
    {
        _studentsRepository.Setup(x => x.Get(It.IsAny<int>())).ReturnsAsync((Student?)null);

        await Assert.ThrowsAsync<DataNotFoundException>(() => Student.Get(_fixture.Create<int>(), _studentsRepository.Object));
    }

    [Theory]
    [InlineData(1, "name", 1, 1)]
    [InlineData(null, "name", 1, 1)]
    [InlineData(null, null, 1, 1)]
    [InlineData(null, null, null, 1)]
    [InlineData(null, null, null, null)]
    public void CreateStudent_ValidInput_Success(int? id, string? name, int? classroomId, int? schoolId)
    {
        var student = Student.Create(id, name, classroomId, schoolId);
        Assert.NotNull(student);
        Assert.Equal(id ?? 0, student.Id);
        Assert.Equal(name, student.Name);
        Assert.Equal(classroomId, student.ClassRoomId);
        Assert.Equal(schoolId, student.SchoolId);
    }

    [Fact]
    public void SetName_ValidInput_Success()
    {
        var student = ValidStudent(_fixture);
        var newName = _fixture.Create<string>();

        student.SetName(newName);

        Assert.Equal(newName, student.Name);
    }

    [Fact]
    public async Task EnsureNoDuplicates_ValidInput_Success()
    {
        _studentsRepository.Setup(x => x.Search(It.IsAny<string?>(), It.IsAny<int?>(), It.IsAny<int?>(), It.IsAny<int>(), It.IsAny<int>())).ReturnsAsync(PagedStudentsEmpty());
        _studentsRepository.Setup(x => x.Create(It.IsAny<Student>())).ReturnsAsync(2);

        var student = ValidStudent(_fixture);

        var output = await student.Create(_studentsRepository.Object, _validationEngine.Object);

        Assert.Equal(2, output);
    }

    [Fact]
    public async Task EnsureNoDuplicates_InvalidInput_ThrowsDuplicateException()
    {
        var student = ValidStudent(_fixture);

        _studentsRepository.Setup(x => x.Search(It.IsAny<string?>(), It.IsAny<int?>(), It.IsAny<int?>(), It.IsAny<int>(), It.IsAny<int>())).ReturnsAsync(PagedStudents(_fixture));
        _studentsRepository.Setup(x => x.Create(It.IsAny<Student>())).ReturnsAsync(2);

        await Assert.ThrowsAsync<DataDuplicateException>(() => student.Create(_studentsRepository.Object, _validationEngine.Object));
    }

    [Fact]
    public async Task EnsureNoDuplicates_InvalidInputNew_ThrowsDuplicateException()
    {
        var student = ValidStudentNew(_fixture);

        _studentsRepository.Setup(x => x.Search(It.IsAny<string?>(), It.IsAny<int?>(), It.IsAny<int?>(), It.IsAny<int>(), It.IsAny<int>())).ReturnsAsync(PagedStudents(_fixture));
        _studentsRepository.Setup(x => x.Create(It.IsAny<Student>())).ReturnsAsync(2);

        await Assert.ThrowsAsync<DataDuplicateException>(() => student.Create(_studentsRepository.Object, _validationEngine.Object));
    }

    [Theory]
    [InlineData(null, null, null)]
    [InlineData("", null, null)]
    [InlineData("s", null, null)]
    [InlineData("12345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890", null, null)]
    [InlineData("name", null, 1)]
    [InlineData("name", 1, null)]
    [InlineData("name", 0, 1)]
    [InlineData("name", 1, -1)]
    [InlineData("name", -1, 1)]
    public void Validator_InvalidObject_ThrowsNotValidException(string? name, int? schoolId, int? classroomId)
    {
        var validationEngine = new ValidationEngine();
        var student = Student.Create(_fixture.Create<int>(), name, schoolId, classroomId);

        Assert.Throws<DataNotValidException>(() => validationEngine.Validate(student));
    }

    private static Student ValidStudent(Fixture fixture) =>
        Student.Create(fixture.Create<int>(), fixture.Create<string>(), fixture.Create<int>(), fixture.Create<int>());

    private static Student ValidStudentNew(Fixture fixture) =>
        Student.Create(null, fixture.Create<string>(), fixture.Create<int>(), fixture.Create<int>());

    private static PagedResponse<Student> PagedStudents(Fixture fixture) =>
        new PagedResponse<Student>
        {
            PageNumber = 1,
            PageSize = 10,
            TotalCount = 10,
            Data = Enumerable.Range(1, 10).Select(s => ValidStudent(fixture)).ToList()
        };

    private static PagedResponse<Student> PagedStudentsEmpty() =>
        new PagedResponse<Student>
        {
            PageNumber = 1,
            PageSize = 10,
            TotalCount = 0,
            Data = new List<Student>()
        };
}
