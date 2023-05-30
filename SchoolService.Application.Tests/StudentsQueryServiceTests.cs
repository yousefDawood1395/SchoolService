using AutoFixture;
using Moq;
using SchoolService.Application.Students;
using SchoolService.Application.Students.DTOs;
using SchoolService.Domain.Shared.Pagination;
using SchoolService.Domain.Shared.Repositories;
using SchoolService.Domain.Students;
using System.Security.AccessControl;

namespace SchoolService.Application.Tests;
public class StudentsQueryServiceTests
{
    private readonly Mock<IStudentsRepository> _studentsRepository;
    private readonly IStudentsQueryService _sut;
    private readonly Fixture _fixture;

    public StudentsQueryServiceTests()
    {
        _studentsRepository = new Mock<IStudentsRepository>();
        _fixture = new Fixture();
        _sut = new StudentsQueryService(_studentsRepository.Object);
    }

    [Fact]
    public async Task Get_ValidInput_success()
    {
        _studentsRepository.Setup(x => x.Get(It.IsAny<int>())).ReturnsAsync(ValidStudent(_fixture));

        var output = await _sut.Get(_fixture.Create<int>());

        Assert.NotNull(output);

        Assert.IsType<StudentDto>(output);
    }

    [Fact]
    public async Task Search_ValidInput_Success()
    {
        _studentsRepository.Setup(x => x.Search(It.IsAny<string>(), It.IsAny<int?>(), It.IsAny<int?>(),
            It.IsAny<int>(), It.IsAny<int>())).ReturnsAsync(PagedStudents(_fixture));

        var output = await _sut.Search(StudentSearchDto(_fixture));

        Assert.NotNull(output);

        Assert.IsType<PagedResponse<StudentDto>>(output);
    }
    private static Student ValidStudent(Fixture fixture) =>
        Student.Create(fixture.Create<int>(),
            fixture.Create<string>(),
            fixture.Create<int>(),
            fixture.Create<int>());

    private static PagedResponse<Student> PagedStudents(Fixture fixture) =>
        new PagedResponse<Student>
        {
            PageNumber = 1,
            PageSize = 10,
            TotalCount = 10,
            Data = Enumerable.Range(1, 10).Select(s => ValidStudent(fixture)).ToList()
        };

    private static StudentSearchDto StudentSearchDto(Fixture fixture) =>
        fixture.Create<StudentSearchDto>();
}
