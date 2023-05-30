using AutoFixture;
using Moq;
using SchoolService.Application.Teachers;
using SchoolService.Application.Teachers.DTOs;
using SchoolService.Domain.Shared.Pagination;
using SchoolService.Domain.Shared.Repositories;
using SchoolService.Domain.Teachers;

namespace SchoolService.Application.Tests;
public class TeachersQueryServiceTests
{
    private readonly Mock<ITeachersRepository> _teachersRepository;
    private readonly ITeachersQueryService _sut;
    private readonly Fixture _fixture;

    public TeachersQueryServiceTests()
    {
        _teachersRepository = new Mock<ITeachersRepository>();
        _sut = new TeachersQueryService(_teachersRepository.Object);

        _fixture = new Fixture();
    }

    [Fact]
    public async Task Get_ValidInput_Success()
    {
        _teachersRepository.Setup(x => x.Get(It.IsAny<int>())).ReturnsAsync(ValidTeacher(_fixture));

        var output = await _sut.Get(_fixture.Create<int>());

        Assert.NotNull(output);

        Assert.IsType<TeacherDto>(output);
    }

    [Fact]
    public async Task Search_ValidInput_Success()
    {
        _teachersRepository.Setup(x => x.Search(It.IsAny<string?>(), It.IsAny<int?>(),
            It.IsAny<int?>(), It.IsAny<int>(), It.IsAny<int>())).ReturnsAsync(PagedTeacher(_fixture));

        var output = await _sut.Search(TeacherSearchDto(_fixture));

        Assert.NotNull(output);

        Assert.IsType<PagedResponse<TeacherDto>>(output);
    }
    private static Teacher ValidTeacher(Fixture fixture)
    {
        var teacher = Teacher.Create(fixture.Create<int>(), fixture.Create<string>());
        teacher.Classrooms.Add(TeacherClassrooms.Create(fixture.Create<int>(),
            teacher.Id,
            fixture.Create<int>(), fixture.Create<int>()));
        return teacher;
    }

    private static PagedResponse<Teacher> PagedTeacher(Fixture fixture) =>
        new PagedResponse<Teacher>
        {
            PageNumber = 1,
            PageSize = 10,
            TotalCount = 10,
            Data = Enumerable.Range(1, 10).Select(s => ValidTeacher(fixture)).ToList()
        };

    private static TeacherSearchDto TeacherSearchDto(Fixture fixture) =>
        fixture.Create<TeacherSearchDto>();
}