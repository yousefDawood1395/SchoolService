using AutoFixture;
using Microsoft.AspNetCore.Mvc;
using Moq;
using SchoolService.Application.Teachers;
using SchoolService.Application.Teachers.DTOs;
using SchoolService.Domain.Shared.Exceptions;
using SchoolService.Domain.Shared.Pagination;
using SchoolService.Presentation.Controllers;

namespace SchoolService.Presentation.Tests;
public class TeachersControllerTests
{
    private readonly Mock<ITeachersService> _teachersService;
    private readonly Mock<ITeachersQueryService> _teacherQueryService;
    private readonly Fixture _fixture;
    private readonly TeachersController _sut;
    public TeachersControllerTests()
    {
        _teachersService = new Mock<ITeachersService>();
        _teacherQueryService = new Mock<ITeachersQueryService>();

        _sut = new TeachersController(_teachersService.Object, _teacherQueryService.Object);
        _fixture = new Fixture();
    }

    [Fact]
    public async Task Create_ValidInput_Success()
    {
        _teachersService.Setup(x => x.Create(It.IsAny<TeacherCreateDto>())).ReturnsAsync(TeacherDto(_fixture));

        var output = await _sut.Create(TeacherCreateDto(_fixture));

        Assert.NotNull(output);

        Assert.IsType<CreatedResult>(output);

        Assert.NotNull(output.Value);

        Assert.IsType<TeacherDto>(output.Value);
    }

    [Fact]
    public async Task Create_InvalidInput_ThrowsDataNotValidException()
    {
        _teachersService.Setup(x => x.Create(It.IsAny<TeacherCreateDto>())).ThrowsAsync(new DataNotValidException());

        await Assert.ThrowsAsync<DataNotValidException>(() => _sut.Create(TeacherCreateDto(_fixture)));
    }

    [Fact]
    public async Task Update_ValidInput_Success()
    {
        _teachersService.Setup(x => x.Update(It.IsAny<TeacherUpdateDto>())).ReturnsAsync(TeacherDto(_fixture));

        var output = await _sut.Update(TeacherUpdateDto(_fixture));

        Assert.NotNull(output);

        Assert.IsType<ActionResult<TeacherDto>>(output);

        Assert.NotNull(output.Result);

        Assert.IsType<OkObjectResult>(output.Result);
    }

    [Fact]
    public async Task Update_InvalidInput_ThrowsDataNotValidException()
    {
        _teachersService.Setup(x => x.Update(It.IsAny<TeacherUpdateDto>())).ThrowsAsync(new DataNotValidException());

        await Assert.ThrowsAsync<DataNotValidException>(() => _sut.Update(TeacherUpdateDto(_fixture)));
    }

    [Fact]
    public async Task Update_InvalidInput_ThrowsDataNotFoundException()
    {
        _teachersService.Setup(x => x.Update(It.IsAny<TeacherUpdateDto>())).ThrowsAsync(new DataNotFoundException());

        await Assert.ThrowsAsync<DataNotFoundException>(() => _sut.Update(TeacherUpdateDto(_fixture)));
    }

    [Fact]
    public async Task Delete_ValidInput_Success()
    {
        _teachersService.Setup(x => x.Delete(It.IsAny<int>())).ReturnsAsync(true);

        var output = await _sut.Delete(_fixture.Create<int>());

        Assert.NotNull(output);

        Assert.IsType<ActionResult<bool>>(output);

        Assert.NotNull(output.Result);

        Assert.IsType<OkObjectResult>(output.Result);
    }

    [Fact]
    public async Task Delete_InvalidInput_ThrowsDataNotFoundException()
    {
        _teachersService.Setup(x => x.Delete(It.IsAny<int>())).ThrowsAsync(new DataNotFoundException());

        await Assert.ThrowsAsync<DataNotFoundException>(() => _sut.Delete(_fixture.Create<int>()));
    }

    [Fact]
    public async Task AssignClass_ValidInput_Success()
    {
        _teachersService.Setup(x => x.AssignTeacherToClassroom(It.IsAny<TeacherAssignClassroomDto>())).ReturnsAsync(true);

        var dto = TeacherAssignClassroomDto(_fixture);

        var output = await _sut.AssignClass(dto.TeacherId, dto);

        Assert.NotNull(output);

        Assert.IsType<ActionResult<bool>>(output);

        Assert.NotNull(output.Result);

        Assert.IsType<OkObjectResult>(output.Result);
    }

    [Fact]
    public async Task AssignClass_InvalidInput_ThrowsDataNotValidException()
    {
        var dto = TeacherAssignClassroomDto(_fixture);
        var teacherId = _fixture.Create<int>();

        await Assert.ThrowsAsync<DataNotValidException>(() => _sut.AssignClass(teacherId, dto));
    }

    [Fact]
    public async Task AssignClass_InvalidInput_ThrowsDataNotFoundException()
    {
        _teachersService.Setup(x => x.AssignTeacherToClassroom(It.IsAny<TeacherAssignClassroomDto>())).ThrowsAsync(new DataNotFoundException());

        var dto = TeacherAssignClassroomDto(_fixture);

        await Assert.ThrowsAsync<DataNotFoundException>(() => _sut.AssignClass(dto.TeacherId, dto));
    }

    [Fact]
    public async Task AssignClass_InvalidInput_ThrowsDataDuplicateException()
    {
        _teachersService.Setup(x => x.AssignTeacherToClassroom(It.IsAny<TeacherAssignClassroomDto>())).ThrowsAsync(new DataDuplicateException());

        var dto = TeacherAssignClassroomDto(_fixture);

        await Assert.ThrowsAsync<DataDuplicateException>(() => _sut.AssignClass(dto.TeacherId, dto));
    }

    [Fact]
    public async Task UnAssignClass_ValidInput_Success()
    {
        _teachersService.Setup(x => x.UnAssignTeacherToClassroom(It.IsAny<TeacherUnAssignClassroomDto>())).ReturnsAsync(true);

        var output = await _sut.UnAssignTeacherToClass(_fixture.Create<int>(), _fixture.Create<int>());

        Assert.NotNull(output);

        Assert.IsType<ActionResult<bool>>(output);

        Assert.NotNull(output.Result);

        Assert.IsType<OkObjectResult>(output.Result);
    }

    [Fact]
    public async Task UnAssignClass_InvalidInput_ThrowsDataNotFoundException()
    {
        _teachersService.Setup(x => x.UnAssignTeacherToClassroom(It.IsAny<TeacherUnAssignClassroomDto>())).ThrowsAsync(new DataNotFoundException());

        await Assert.ThrowsAsync<DataNotFoundException>(() => _sut.UnAssignTeacherToClass(_fixture.Create<int>(), _fixture.Create<int>()));
    }

    [Fact]
    public async Task Get_ValidInput_Success()
    {
        _teacherQueryService.Setup(x => x.Get(It.IsAny<int>())).ReturnsAsync(TeacherDto(_fixture));

        var output = await _sut.Get(_fixture.Create<int>());

        Assert.NotNull(output);

        Assert.IsType<ActionResult<TeacherDto>>(output);

        Assert.NotNull(output.Result);

        Assert.IsType<OkObjectResult>(output.Result);
    }

    [Fact]
    public async Task Get_InvalidInput_ThrowsDataNotFoundException()
    {
        _teacherQueryService.Setup(x => x.Get(It.IsAny<int>())).ThrowsAsync(new DataNotFoundException());

        await Assert.ThrowsAsync<DataNotFoundException>(() => _sut.Get(_fixture.Create<int>()));
    }

    [Fact]
    public async Task Search_ValidInput_Success()
    {
        _teacherQueryService.Setup(x => x.Search(It.IsAny<TeacherSearchDto>())).ReturnsAsync(PagedTeachers(_fixture));

        var output = await _sut.Search(TeacherSearchDto(_fixture));

        Assert.NotNull(output);

        Assert.IsType<ActionResult<PagedResponse<TeacherDto>>>(output);

        Assert.NotNull(output.Result);

        Assert.IsType<OkObjectResult>(output.Result);
    }
    private static TeacherDto TeacherDto(Fixture fixture) =>
        fixture.Create<TeacherDto>();

    private static TeacherCreateDto TeacherCreateDto(Fixture fixture) =>
        fixture.Create<TeacherCreateDto>();

    private static TeacherUpdateDto TeacherUpdateDto(Fixture fixture) =>
        fixture.Create<TeacherUpdateDto>();

    private static TeacherAssignClassroomDto TeacherAssignClassroomDto(Fixture fixture) =>
        fixture.Create<TeacherAssignClassroomDto>();

    private static TeacherSearchDto TeacherSearchDto(Fixture fixture) =>
        fixture.Create<TeacherSearchDto>();
    private static PagedResponse<TeacherDto> PagedTeachers(Fixture fixture) =>
        new PagedResponse<TeacherDto>
        {
            PageNumber = 1,
            PageSize = 10,
            TotalCount = 10,
            Data = Enumerable.Range(1, 10).Select(s => TeacherDto(fixture)).ToList()
        };

}
