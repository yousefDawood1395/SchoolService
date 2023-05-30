using AutoFixture;
using Microsoft.AspNetCore.Mvc;
using Moq;
using SchoolService.Application.Students;
using SchoolService.Application.Students.DTOs;
using SchoolService.Domain.Shared.Exceptions;
using SchoolService.Domain.Shared.Pagination;
using SchoolService.Presentation.Controllers;

namespace SchoolService.Presentation.Tests;
public class StudentsControllerTests
{
    private readonly Mock<IStudentsService> _studentsService;
    private readonly Mock<IStudentsQueryService> _studentsQueryService;
    private readonly StudentsController _sut;
    private readonly Fixture _fixture;

    public StudentsControllerTests()
    {
        _studentsService = new Mock<IStudentsService>();
        _studentsQueryService = new Mock<IStudentsQueryService>();

        _sut = new StudentsController(_studentsService.Object, _studentsQueryService.Object);
        _fixture = new Fixture();
    }

    [Fact]
    public async Task Create_ValidInput_Success()
    {
        _studentsService.Setup(x => x.Create(It.IsAny<StudentCreateDto>())).ReturnsAsync(StudentDto(_fixture));

        var output = await _sut.Create(StudentCreateDto(_fixture));

        Assert.NotNull(output);

        Assert.IsType<CreatedResult>(output);

        Assert.NotNull(output.Value);

        Assert.IsType<StudentDto>(output.Value);
    }

    [Fact]
    public async Task Create_InvalidInput_ThrowsDataNotValidException()
    {
        _studentsService.Setup(x => x.Create(It.IsAny<StudentCreateDto>())).ThrowsAsync(new DataNotValidException());

        await Assert.ThrowsAsync<DataNotValidException>(() => _sut.Create(StudentCreateDto(_fixture)));
    }

    [Fact]
    public async Task Update_ValidInput_Success()
    {
        _studentsService.Setup(x => x.Update(It.IsAny<StudentUpdateDto>())).ReturnsAsync(StudentDto(_fixture));

        var output = await _sut.Update(StudentUpdateDto(_fixture));

        Assert.NotNull(output);

        Assert.IsType<ActionResult<StudentDto>>(output);

        Assert.NotNull(output.Result);

        Assert.IsType<OkObjectResult>(output.Result);
    }

    [Fact]
    public async Task Update_InvalidInput_ThrowsDataNotValidException()
    {
        _studentsService.Setup(x => x.Update(It.IsAny<StudentUpdateDto>())).ThrowsAsync(new DataNotValidException());

        await Assert.ThrowsAsync<DataNotValidException>(() => _sut.Update(StudentUpdateDto(_fixture)));
    }

    [Fact]
    public async Task Update_InvalidInput_ThrowsDataNotFoundException()
    {
        _studentsService.Setup(x => x.Update(It.IsAny<StudentUpdateDto>())).ThrowsAsync(new DataNotFoundException());

        await Assert.ThrowsAsync<DataNotFoundException>(() => _sut.Update(StudentUpdateDto(_fixture)));
    }

    [Fact]
    public async Task Delet_ValidInput_Success()
    {
        _studentsService.Setup(x => x.Delete(It.IsAny<int>())).ReturnsAsync(true);

        var output = await _sut.Delete(_fixture.Create<int>());

        Assert.NotNull(output);

        Assert.IsType<ActionResult<bool>>(output);

        Assert.NotNull(output.Result);

        Assert.IsType<OkObjectResult>(output.Result);
    }

    [Fact]
    public async Task Delet_InValidInput_ThrowsDataNotFound()
    {
        _studentsService.Setup(x => x.Delete(It.IsAny<int>())).ThrowsAsync(new DataNotFoundException());

        await Assert.ThrowsAsync<DataNotFoundException>(() => _sut.Delete(_fixture.Create<int>()));
    }

    [Fact]
    public async Task SetClass_ValidInput_Success()
    {
        _studentsService.Setup(x => x.SetClass(It.IsAny<StudentSetClassroomDto>())).ReturnsAsync(true);

        var dto = StudentSetClassroomDto(_fixture);

        var output = await _sut.SetClass(dto.StudentId, dto);

        Assert.NotNull(output);

        Assert.IsType<ActionResult<bool>>(output);

        Assert.NotNull(output.Result);

        Assert.IsType<OkObjectResult>(output.Result);
    }

    [Fact]
    public async Task SetClass_InvalidInput_ThrowsDataNotValidException()
    {
        _studentsService.Setup(x => x.SetClass(It.IsAny<StudentSetClassroomDto>())).ThrowsAsync(new DataNotValidException());

        var dto = StudentSetClassroomDto(_fixture);

        await Assert.ThrowsAsync<DataNotValidException>(() => _sut.SetClass(dto.StudentId, dto));
    }

    [Fact]
    public async Task SetClass_StudentIdMismatch_ThrowsDataNotValidException()
    {
        var dto = StudentSetClassroomDto(_fixture);
        var studentId = _fixture.Create<int>();

        await Assert.ThrowsAsync<DataNotValidException>(() => _sut.SetClass(studentId, dto));
    }

    [Fact]
    public async Task SetClass_InvalidInput_ThrowsDataNotFoundException()
    {
        _studentsService.Setup(x => x.SetClass(It.IsAny<StudentSetClassroomDto>())).ThrowsAsync(new DataNotFoundException());

        var dto = StudentSetClassroomDto(_fixture);


        await Assert.ThrowsAsync<DataNotFoundException>(() => _sut.SetClass(dto.StudentId, dto));
    }

    [Fact]
    public async Task Get_ValidInput_Success()
    {
        _studentsQueryService.Setup(x => x.Get(It.IsAny<int>())).ReturnsAsync(StudentDto(_fixture));

        var output = await _sut.Get(_fixture.Create<int>());

        Assert.NotNull(output);

        Assert.IsType<ActionResult<StudentDto>>(output);

        Assert.NotNull(output.Result);

        Assert.IsType<OkObjectResult>(output.Result);
    }

    [Fact]
    public async Task Get_InvalidInput_ThrowsDataNotFoundException()
    {
        _studentsQueryService.Setup(x => x.Get(It.IsAny<int>())).ThrowsAsync(new DataNotFoundException());

        await Assert.ThrowsAsync<DataNotFoundException>(() => _sut.Get(_fixture.Create<int>()));
    }

    [Fact]
    public async Task Search_ValidInput_Success()
    {
        _studentsQueryService.Setup(x => x.Search(It.IsAny<StudentSearchDto>())).ReturnsAsync(PagedStudents(_fixture));

        var output = await _sut.Search(StudentSearchDto(_fixture));

        Assert.NotNull(output);

        Assert.IsType<ActionResult<PagedResponse<StudentDto>>>(output);

        Assert.NotNull(output.Result);

        Assert.IsType<OkObjectResult>(output.Result);
    }

    private static StudentDto StudentDto(Fixture fixture) =>
        fixture.Create<StudentDto>();

    private static StudentCreateDto StudentCreateDto(Fixture fixture) =>
        fixture.Create<StudentCreateDto>();

    private static StudentUpdateDto StudentUpdateDto(Fixture fixture) =>
        fixture.Create<StudentUpdateDto>();
    private static StudentSetClassroomDto StudentSetClassroomDto(Fixture fixture) =>
        fixture.Create<StudentSetClassroomDto>();

    private static PagedResponse<StudentDto> PagedStudents(Fixture fixture) =>
        new PagedResponse<StudentDto>
        {
            PageNumber = 1,
            PageSize = 10,
            TotalCount = 10,
            Data = Enumerable.Range(1, 10).Select(s => StudentDto(fixture)).ToList()
        };

    private static StudentSearchDto StudentSearchDto(Fixture fixture) =>
        fixture.Create<StudentSearchDto>();
}
