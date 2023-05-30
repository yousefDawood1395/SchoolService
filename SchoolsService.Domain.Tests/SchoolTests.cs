using AutoFixture;
using Moq;
using SchoolService.Domain.Schools;
using SchoolService.Domain.Shared.Exceptions;
using SchoolService.Domain.Shared.Pagination;
using SchoolService.Domain.Shared.Repositories;
using SchoolService.Domain.Shared.Validation;

namespace SchoolsService.Domain.Tests;
public class SchoolTests
{
    private readonly Fixture _fixture;
    private readonly Mock<ISchoolsRepository> _schoolsRepository;
    private readonly Mock<IValidationEngine> _validationEngine;

    public SchoolTests()
    {
        _fixture = new Fixture();
        _schoolsRepository = new Mock<ISchoolsRepository>();
        _validationEngine = new Mock<IValidationEngine>();
        _validationEngine.Setup(x => x.Validate(It.IsAny<School>(), true));
        _validationEngine.Setup(x => x.Validate(It.IsAny<ClassRoom>(), true));
    }

    [Theory]
    [InlineData(1, "name", "2000-01-01")]
    [InlineData(null, "name1", "2000-01-01")]
    [InlineData(1, "name2", null)]
    [InlineData(null, null, null)]
    public void SchoolsCreate_ValidInputs_Success(int? id, string? name, string? openDate)
    {
        DateTime? parsedOpenDate = null;
        if (!string.IsNullOrEmpty(openDate))
        {
            parsedOpenDate = DateTime.Parse(openDate);
        }

        var school = School.Create(id, name, parsedOpenDate);

        Assert.NotNull(school);

        Assert.Equal(id ?? 0, school.Id);
        Assert.Equal(name, school.Name);
        Assert.Equal(parsedOpenDate, school.OpenDate);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("s")]
    [InlineData("12345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890")]
    public void Validator_Tests_Success(string? name)
    {
        var school = School.Create(null, name, null);

        Assert.NotNull(school);

        var validator = new ValidationEngine();

        Assert.Throws<DataNotValidException>(() => validator.Validate(school));
    }

    [Theory]
    [InlineData(null, null)]
    [InlineData("n", null)]
    [InlineData("12345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890", null)]
    [InlineData("class", 0)]
    [InlineData("class", -1)]
    public void ClassroomValidator_Tests_Success(string? name, int? schoolId)
    {
        var validator = new ValidationEngine();
        var classRoom = ClassRoom.Create(null, name, schoolId);

        Assert.Throws<DataNotValidException>(() => validator.Validate(classRoom, true));
    }

    [Theory]
    [InlineData(1)]
    [InlineData(0)]
    [InlineData(10)]
    [InlineData(50)]
    public async void ClassesCount_Tests_Success(int classCount)
    {
        _schoolsRepository.Setup(x => x.UpdateSchool(It.IsAny<School>())).ReturnsAsync(true);
        var school = ValidSchool(_fixture);

        for (int i = 0; i < classCount; i++)
        {
            await school.AddClass(ValidClass(_fixture), _schoolsRepository.Object, _validationEngine.Object);
        }

        Assert.NotNull(school);

        Assert.Equal(classCount, school.ClassesCount);
    }

    [Fact]
    public async Task Create_ValidInput_Success()
    {
        _schoolsRepository.Setup(x => x.CreateSchool(It.IsAny<School>())).ReturnsAsync(1);
        _schoolsRepository.Setup(x => x.SearchSchoolsByName(It.IsAny<string?>(), It.IsAny<int>(), It.IsAny<int>()))
            .ReturnsAsync(PagedSchoolEmpty());

        var school = ValidSchool(_fixture);

        var output = await school.Create(_validationEngine.Object, _schoolsRepository.Object);

        Assert.Equal(1, output);
    }

    [Fact]
    public async Task Update_ValidInput_Success()
    {
        _schoolsRepository.Setup(x => x.UpdateSchool(It.IsAny<School>())).ReturnsAsync(true);
        _schoolsRepository.Setup(x => x.SearchSchoolsByName(It.IsAny<string?>(), It.IsAny<int>(), It.IsAny<int>()))
            .ReturnsAsync(PagedSchoolEmpty());

        var school = ValidSchool(_fixture);

        var output = await school.Update(_validationEngine.Object, _schoolsRepository.Object);

        Assert.True(output);
    }

    [Fact]
    public async Task AddClass_ValidInput_Success()
    {
        _schoolsRepository.Setup(x => x.UpdateSchool(It.IsAny<School>())).ReturnsAsync(true);

        var school = ValidSchool(_fixture);

        var classRoom = ValidClass(_fixture);

        var output = await school.AddClass(classRoom, _schoolsRepository.Object, _validationEngine.Object);

        Assert.Equal(output, classRoom.Id);
        Assert.Equal(1, school.ClassesCount);
        Assert.True(school.Classes.Contains(classRoom));
    }

    [Fact]
    public async Task UpdateClass_ValidInput_Success()
    {
        var school = ValidSchool(_fixture);
        var classRoom = ValidClass(_fixture);

        school.Classes.Add(classRoom);

        _schoolsRepository.Setup(x => x.UpdateClass(It.IsAny<ClassRoom>())).ReturnsAsync(true);

        var output = await school.UpdateClass(classRoom, _schoolsRepository.Object, _validationEngine.Object);

        Assert.IsType<bool>(output);

        Assert.True(output);
    }

    [Fact]
    public async Task UpdateClass_InvalidInput_ThrowsDataNotFoundException()
    {
        var school = ValidSchool(_fixture);
        var classRoom = ValidClass(_fixture);

        await Assert.ThrowsAsync<DataNotFoundException>(() => school.UpdateClass(classRoom, _schoolsRepository.Object, _validationEngine.Object));
    }

    [Fact]
    public void SetName_ValidInput_Success()
    {
        var school = ValidSchool(_fixture);
        var newName = _fixture.Create<string>();
        school.SetName(newName);

        Assert.Equal(newName, school.Name);
    }

    [Fact]
    public void SetClassName_ValidInput_Success()
    {
        var school = ValidSchool(_fixture);
        var classRoom = ValidClass(_fixture);
        var newName = _fixture.Create<string>();

        school.Classes.Add(classRoom);

        school.SetClassName(classRoom.Id, newName);

        Assert.Equal(newName, classRoom.Name);
    }

    [Fact]
    public void SetClassName_InvalidInput_ThrowsDataNotFoundException()
    {
        var school = ValidSchool(_fixture);
        var newName = _fixture.Create<string>();

        Assert.Throws<DataNotFoundException>(() => school.SetClassName(_fixture.Create<int>(), newName));
    }

    [Fact]
    public void SetOpenDate_ValidInput_Success()
    {
        var school = ValidSchool(_fixture);

        var newDate = _fixture.Create<DateTime>();

        school.SetOpenDate(newDate);

        Assert.Equal(newDate, school.OpenDate);
    }

    [Fact]
    public async Task EnsureNoDuplicate_InvalidInput_ThrowsDataDuplicateException()
    {
        _schoolsRepository.Setup(x => x.CreateSchool(It.IsAny<School>())).ReturnsAsync(1);
        _schoolsRepository.Setup(x => x.SearchSchoolsByName(It.IsAny<string?>(), It.IsAny<int>(), It.IsAny<int>()))
            .ReturnsAsync(PagedSchools(_fixture));

        var school = ValidSchool(_fixture);

        await Assert.ThrowsAsync<DataDuplicateException>(() => school.Create(_validationEngine.Object, _schoolsRepository.Object));
    }

    [Fact]
    public async Task EnsureNoDuplicate_InvalidInputNewSchool_ThrowsDataDuplicateException()
    {
        _schoolsRepository.Setup(x => x.CreateSchool(It.IsAny<School>())).ReturnsAsync(1);
        _schoolsRepository.Setup(x => x.SearchSchoolsByName(It.IsAny<string?>(), It.IsAny<int>(), It.IsAny<int>()))
            .ReturnsAsync(PagedSchools(_fixture));

        var school = ValidSchoolNew(_fixture);

        await Assert.ThrowsAsync<DataDuplicateException>(() => school.Create(_validationEngine.Object, _schoolsRepository.Object));
    }


    [Fact]
    public async Task EnsureNoClassroomDuplicates_InvalidInput_ThrowsDataDuplicateException()
    {
        var school = ValidSchool(_fixture);
        var classRoom = ValidClass(_fixture);

        _schoolsRepository.Setup(x => x.UpdateSchool(It.IsAny<School>())).ReturnsAsync(true);

        await school.AddClass(classRoom, _schoolsRepository.Object, _validationEngine.Object);

        string? duplicateName = classRoom.Name;

        classRoom = ValidClass(_fixture);
        classRoom.SetName(duplicateName);

        await Assert.ThrowsAsync<DataDuplicateException>(() => school.AddClass(classRoom, _schoolsRepository.Object, _validationEngine.Object));
    }

    [Fact]
    public async Task EnsureNoClassroomDuplicates_InvalidInputNewClass_ThrowsDataDuplicateException()
    {
        var school = ValidSchool(_fixture);
        var classRoom = ValidClassroomNew(_fixture);

        _schoolsRepository.Setup(x => x.UpdateSchool(It.IsAny<School>())).ReturnsAsync(true);

        await school.AddClass(classRoom, _schoolsRepository.Object, _validationEngine.Object);

        await Assert.ThrowsAsync<DataDuplicateException>(() => school.AddClass(classRoom, _schoolsRepository.Object, _validationEngine.Object));
    }

    [Fact]
    public void CreateSchool_ValidInput_Success()
    {
        int id = _fixture.Create<int>();
        string name = _fixture.Create<string>();
        DateTime openDate = _fixture.Create<DateTime>();

        var school = School.Create(id, name, openDate);

        Assert.Equal(id, school.Id);
        Assert.Equal(name, school.Name);
        Assert.Equal(openDate, school.OpenDate);
    }

    [Fact]
    public async Task Get_ValidInput_Success()
    {
        var school = ValidSchool(_fixture);
        _schoolsRepository.Setup(x => x.GetSchool(It.IsAny<int>())).ReturnsAsync(school);

        var output = await School.Get(_fixture.Create<int>(), _schoolsRepository.Object);

        Assert.Equal(school.Id, output.Id);
        Assert.Equal(school.Name, output.Name);
        Assert.Equal(school.OpenDate, output.OpenDate);
    }
    [Fact]
    public async Task Get_InvalidInput_ThrowsNotFoundException()
    {
        _schoolsRepository.Setup(x => x.GetSchool(It.IsAny<int>())).ReturnsAsync((School?)null);

        await Assert.ThrowsAsync<DataNotFoundException>(() => School.Get(_fixture.Create<int>(), _schoolsRepository.Object));
    }

    [Fact]
    public async Task SearchSchools_ValidInput_Success()
    {
        var pagedSchools = PagedSchools(_fixture);

        _schoolsRepository.Setup(x => x.SearchSchoolsByName(It.IsAny<string?>(), It.IsAny<int>(), It.IsAny<int>())).ReturnsAsync(pagedSchools);

        var output = await School.SearchSchools(_fixture.Create<string>(), _fixture.Create<int>(), _fixture.Create<int>(), _schoolsRepository.Object);

        Assert.Equal(pagedSchools.TotalCount, output.TotalCount);
        Assert.Equal(pagedSchools.Data, output.Data);

    }
    private static ClassRoom ValidClass(Fixture fixture) =>
        ClassRoom.Create(fixture.Create<int>(), fixture.Create<string>(), fixture.Create<int>());

    private static School ValidSchool(Fixture fixture) =>
        School.Create(fixture.Create<int>(), fixture.Create<string>(), fixture.Create<DateTime>());

    private static School ValidSchoolNew(Fixture fixture) =>
        School.Create(null, fixture.Create<string>(), fixture.Create<DateTime>());

    private static ClassRoom ValidClassroomNew(Fixture fixture) =>
        ClassRoom.Create(null, fixture.Create<string>(), fixture.Create<int>());

    private static PagedResponse<School> PagedSchoolEmpty() =>
        new PagedResponse<School>
        {
            Data = new List<School>(),
            PageNumber = 1,
            PageSize = 10,
            TotalCount = 0
        };

    private static PagedResponse<School> PagedSchools(Fixture fixture) =>
    new PagedResponse<School>
    {
        PageNumber = 1,
        PageSize = 10,
        TotalCount = 10,
        Data = Enumerable.Range(1, 10).Select(s => ValidSchool(fixture)).ToList()
    };
}
