using AutoFixture;
using FluentValidation.Results;
using Moq;
using SchoolService.Application.Schools;
using SchoolService.Application.Schools.Dtos;
using SchoolService.Domain.DomainServices;
using SchoolService.Domain.Schools;
using SchoolService.Domain.Shared.Pagination;
using SchoolService.Domain.Shared.Repositories;
using SchoolService.Domain.Shared.Validation;

namespace SchoolService.Application.Tests;
public class SchoolServiceTests
{
    private readonly Mock<ISchoolsRepository> _schoolsRepositoryl;
    private readonly Mock<IValidationEngine> _validationEngine;
    private readonly Mock<ISchoolsDomainService> _schoolsDomainService;
    private readonly ISchoolsService _sut;
    private readonly Fixture _fixture;
    public SchoolServiceTests()
    {
        _schoolsRepositoryl = new Mock<ISchoolsRepository>();
        _validationEngine = new Mock<IValidationEngine>();
        _validationEngine.Setup(x => x.Validate(It.IsAny<School>(), true)).Returns((List<ValidationFailure>?)null);
        _schoolsDomainService = new Mock<ISchoolsDomainService>();
        _fixture = new Fixture();
        _sut = new SchoolsService(_schoolsRepositoryl.Object, _validationEngine.Object, _schoolsDomainService.Object);
    }

    [Fact]
    public async Task Create_ValidInput_Success()
    {
        _schoolsRepositoryl.Setup(x => x.CreateSchool(It.IsAny<School>())).ReturnsAsync(_fixture.Create<int>());
        _schoolsRepositoryl.Setup(x => x.SearchSchoolsByName(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int>()))
            .ReturnsAsync(PagedSchoolsEmpty());

        var output = await _sut.Create(SchoolCreateDto(_fixture));

        Assert.NotNull(output);

        Assert.IsType<SchoolDto>(output);
    }

    [Fact]
    public async Task Update_ValidInput_Success()
    {
        _schoolsRepositoryl.Setup(x => x.UpdateSchool(It.IsAny<School>())).ReturnsAsync(true);
        _schoolsRepositoryl.Setup(x => x.GetSchool(It.IsAny<int>())).ReturnsAsync(ValidSchool(_fixture));

        _schoolsRepositoryl.Setup(x => x.SearchSchoolsByName(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int>()))
            .ReturnsAsync(PagedSchoolsEmpty());

        var output = await _sut.Update(SchoolUpdateDto(_fixture));

        Assert.NotNull(output);

        Assert.IsType<SchoolDto>(output);
    }

    [Fact]
    public async Task Delete_validInput_Success()
    {
        _schoolsDomainService.Setup(x => x.DeleteSchool(It.IsAny<int>())).ReturnsAsync(true);

        var output = await _sut.Delete(_fixture.Create<int>());

        Assert.IsType<bool>(output);

        Assert.True(output);
    }

    [Fact]
    public async Task AddClass_ValidInput_Success()
    {
        _schoolsRepositoryl.Setup(x => x.GetSchool(It.IsAny<int>())).ReturnsAsync(ValidSchool(_fixture));

        _schoolsRepositoryl.Setup(x => x.UpdateSchool(It.IsAny<School>())).ReturnsAsync(true);

        var output = await _sut.AddClass(ClassRoomAddDto(_fixture));

        Assert.NotNull(output);

        Assert.IsType<SchoolDto>(output);
    }

    [Fact]
    public async Task DeleteClass_ValidInput_Success()
    {
        _schoolsDomainService.Setup(x => x.DeleteClass(It.IsAny<int>(), It.IsAny<int>())).ReturnsAsync(true);

        var output = await _sut.DeleteClass(ClassRoomDeleteDto(_fixture));

        Assert.IsType<bool>(output);

        Assert.True(output);
    }

    [Fact]
    public async Task UpdateClassroom_validInput_Success()
    {
        var updateClassDto = ClassRoomUpdateDto(_fixture);
        var school = ValidSchool(_fixture);
        updateClassDto.SchoolId = school.Id;

        _schoolsRepositoryl.Setup(x => x.UpdateSchool(It.IsAny<School>())).ReturnsAsync(true);

        await school.AddClass(ClassRoom.Create(updateClassDto.ClassRoomId, updateClassDto.Name, school.Id), _schoolsRepositoryl.Object, _validationEngine.Object);

        _schoolsRepositoryl.Setup(x => x.GetSchool(It.IsAny<int>())).ReturnsAsync(school);

        var output = await _sut.UpdateClassroom(updateClassDto);

        Assert.NotNull(output);

        Assert.IsType<SchoolDto>(output);
    }
    private static School ValidSchool(Fixture fixture) =>
        School.Create(fixture.Create<int>(), fixture.Create<string>(), fixture.Create<DateTime>());

    private static PagedResponse<School> PagedSchoolsEmpty() =>
        new PagedResponse<School>
        {
            PageNumber = 1,
            PageSize = 10,
            TotalCount = 0,
            Data = new List<School>()
        };
    private static SchoolCreateDto SchoolCreateDto(Fixture fixture) =>
        fixture.Create<SchoolCreateDto>();

    private static SchoolUpdateDto SchoolUpdateDto(Fixture fixture) =>
        fixture.Create<SchoolUpdateDto>();

    private static ClassRoomAddDto ClassRoomAddDto(Fixture fixture) =>
        fixture.Create<ClassRoomAddDto>();

    private static ClassRoomUpdateDto ClassRoomUpdateDto(Fixture fixture) =>
        fixture.Create<ClassRoomUpdateDto>();

    private static ClassRoomDeleteDto ClassRoomDeleteDto(Fixture fixture) =>
        fixture.Create<ClassRoomDeleteDto>();
}