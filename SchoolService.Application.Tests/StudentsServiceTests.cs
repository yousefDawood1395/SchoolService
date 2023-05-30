using AutoFixture;
using Moq;
using SchoolService.Application.Students;
using SchoolService.Application.Students.DTOs;
using SchoolService.Domain.DomainServices;
using SchoolService.Domain.Shared.Pagination;
using SchoolService.Domain.Shared.Repositories;
using SchoolService.Domain.Shared.Validation;
using SchoolService.Domain.Students;

namespace SchoolService.Application.Tests;
public class StudentsServiceTests
{
    private readonly Mock<IStudentsRepository> _studentsRepository;
    private readonly Mock<IStudentsDomainService> _studentsDomainService;
    private readonly Mock<ISchoolsRepository> _schoolsRepository;
    private readonly Mock<IValidationEngine> _validationEngine;
    private readonly IStudentsService _sut;
    private readonly Fixture _fixture;

    public StudentsServiceTests()
    {
        _studentsRepository = new Mock<IStudentsRepository>();
        _studentsDomainService = new Mock<IStudentsDomainService>();
        _schoolsRepository = new Mock<ISchoolsRepository>();
        _validationEngine = new Mock<IValidationEngine>();
        _fixture = new Fixture();

        _sut = new StudentsService(_studentsRepository.Object, _studentsDomainService.Object,
            _schoolsRepository.Object, _validationEngine.Object);
    }

    private static Student ValidStudent(Fixture fixture) =>
        Student.Create(fixture.Create<int>(),
            fixture.Create<string>(),
            fixture.Create<int>(),
            fixture.Create<int>());

    private static PagedResponse<Student> PagedStudentesEmpty() =>
        new PagedResponse<Student>
        {
            PageNumber = 1,
            PageSize = 10,
            TotalCount = 10,
            Data = new List<Student>()
        };

    [Fact]
    public async Task Create_ValidInput_Success()
    {
        _studentsRepository.Setup(x => x.Search(It.IsAny<string?>(), It.IsAny<int?>(), It.IsAny<int?>(),
            It.IsAny<int>(), It.IsAny<int>())).ReturnsAsync(PagedStudentesEmpty());

        _studentsRepository.Setup(x => x.Create(It.IsAny<Student>())).ReturnsAsync(_fixture.Create<int>());

        var output = await _sut.Create(StudentCreateDto(_fixture));

        Assert.NotNull(output);

        Assert.IsType<StudentDto>(output);
    }

    [Fact]
    public async Task Update_ValidInput_Success()
    {
        _studentsRepository.Setup(x => x.Search(It.IsAny<string?>(), It.IsAny<int?>(), It.IsAny<int?>(),
            It.IsAny<int>(), It.IsAny<int>())).ReturnsAsync(PagedStudentesEmpty());

        _studentsRepository.Setup(x => x.Get(It.IsAny<int>())).ReturnsAsync(ValidStudent(_fixture));

        _studentsRepository.Setup(x => x.Update(It.IsAny<Student>())).ReturnsAsync(true);

        var output = await _sut.Update(StudentUpdateDto(_fixture));

        Assert.NotNull(output);

        Assert.IsType<StudentDto>(output);
    }

    [Fact]
    public async Task SetClass_ValidInput_Success()
    {
        _studentsDomainService.Setup(x => x.SetClass(It.IsAny<int>(), It.IsAny<int?>(), It.IsAny<int?>())).ReturnsAsync(true);

        var output = await _sut.SetClass(StudentSetClassroomDto(_fixture));

        Assert.IsType<bool>(output);

        Assert.True(output);
    }

    [Fact]
    public async Task Delete_ValidInput_Success()
    {
        _studentsRepository.Setup(x => x.Get(It.IsAny<int>())).ReturnsAsync(ValidStudent(_fixture));

        _studentsRepository.Setup(x => x.Delete(It.IsAny<Student>())).ReturnsAsync(true);

        var output = await _sut.Delete(_fixture.Create<int>());

        Assert.IsType<bool>(output);

        Assert.True(output);
    }

    private static StudentCreateDto StudentCreateDto(Fixture fixture) =>
        fixture.Create<StudentCreateDto>();

    private static StudentUpdateDto StudentUpdateDto(Fixture fixture) =>
        fixture.Create<StudentUpdateDto>();

    private static StudentSetClassroomDto StudentSetClassroomDto(Fixture fixture) =>
        fixture.Create<StudentSetClassroomDto>();
}