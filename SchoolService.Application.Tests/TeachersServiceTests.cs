using AutoFixture;
using FluentValidation.Results;
using Moq;
using SchoolService.Application.Teachers;
using SchoolService.Application.Teachers.DTOs;
using SchoolService.Domain.DomainServices;
using SchoolService.Domain.Shared.Pagination;
using SchoolService.Domain.Shared.Repositories;
using SchoolService.Domain.Shared.Validation;
using SchoolService.Domain.Teachers;

namespace SchoolService.Application.Tests;
public class TeachersServiceTests
{
    private readonly Mock<ITeachersRepository> _teachersRepository;
    private readonly Mock<IValidationEngine> _validationEngine;
    private readonly Mock<ITeachersDomainService> _teachersDomainService;
    private readonly ITeachersService _sut;
    private readonly Fixture _fixture;

    public TeachersServiceTests()
    {
        _teachersRepository = new Mock<ITeachersRepository>();
        _validationEngine = new Mock<IValidationEngine>();
        _teachersDomainService = new Mock<ITeachersDomainService>();
        _validationEngine.Setup(x => x.Validate(It.IsAny<Teacher>(), It.IsAny<bool>())).Returns((List<ValidationFailure>?)null);
        _fixture = new Fixture();
        _sut = new TeachersService(_teachersRepository.Object, _validationEngine.Object, _teachersDomainService.Object);
    }

    [Fact]
    public async Task Create_ValidInput_success()
    {
        _teachersRepository.Setup(x => x.Create(It.IsAny<Teacher>())).ReturnsAsync(_fixture.Create<int>());

        var output = await _sut.Create(TeacherCreateDto(_fixture));

        Assert.NotNull(output);

        Assert.IsType<TeacherDto>(output);
    }

    [Fact]
    public async Task Update_ValidInput_Success()
    {
        _teachersRepository.Setup(x => x.Get(It.IsAny<int>())).ReturnsAsync(ValidTeacher(_fixture));

        _teachersRepository.Setup(x => x.Update(It.IsAny<Teacher>())).ReturnsAsync(true);

        var output = await _sut.Update(TeacherUpdateDto(_fixture));

        Assert.NotNull(output);

        Assert.IsType<TeacherDto>(output);
    }

    [Fact]
    public async Task Delete_ValidInput_Success()
    {
        _teachersRepository.Setup(x => x.Get(It.IsAny<int>())).ReturnsAsync(ValidTeacher(_fixture));

        _teachersRepository.Setup(x => x.Delete(It.IsAny<Teacher>())).ReturnsAsync(true);

        var output = await _sut.Delete(_fixture.Create<int>());

        Assert.IsType<bool>(output);

        Assert.True(output);
    }

    [Fact]
    public async Task AssignTeacherToClassroom_ValidInput_Success()
    {
        _teachersDomainService.Setup(x => x.AssignTeacherToClass(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>())).ReturnsAsync(true);

        var output = await _sut.AssignTeacherToClassroom(TeacherAssignClassroomDto(_fixture));

        Assert.IsType<bool>(output);

        Assert.True(output);
    }

    [Fact]
    public async Task UnAssignTeacherToClassroom_ValidInput_Success()
    {
        _teachersDomainService.Setup(x => x.UnAssignTeacherFromClass(It.IsAny<int>(), It.IsAny<int>())).ReturnsAsync(true);

        var output = await _sut.UnAssignTeacherToClassroom(TeacherUnAssignClassroomDto(_fixture));

        Assert.IsType<bool>(output);

        Assert.True(output);
    }
    private static Teacher ValidTeacher(Fixture fixture) =>
    Teacher.Create(fixture.Create<int>(), fixture.Create<string>());

    private static TeacherCreateDto TeacherCreateDto(Fixture fixture) =>
        fixture.Create<TeacherCreateDto>();

    private static TeacherUpdateDto TeacherUpdateDto(Fixture fixture) =>
        fixture.Create<TeacherUpdateDto>();

    private static TeacherAssignClassroomDto TeacherAssignClassroomDto(Fixture fixture) =>
        fixture.Create<TeacherAssignClassroomDto>();

    private static TeacherUnAssignClassroomDto TeacherUnAssignClassroomDto(Fixture fixture) =>
        fixture.Create<TeacherUnAssignClassroomDto>();
}
