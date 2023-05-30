using AutoFixture;
using SchoolService.DataAccess;
using SchoolService.Domain.Schools;
using SchoolService.Infrastructure.Repositories;
using SchoolService.Infrastructure.Tests.SchoolsDbContextFixtures;
using System.Collections.Immutable;

namespace SchoolService.Infrastructure.Tests;
[Collection(nameof(SchoolsDbContextFixtureCollectionDefinition))]
public class SchoolsRepositoryTests
{
    private readonly SchoolsDbContext _dbContext;
    private readonly SchoolsRepository _sut;
    private readonly Fixture _fixture;
    public SchoolsRepositoryTests(SchoolsDbContextFixture dbContextFixture)
    {
        _fixture = new Fixture();
        _dbContext = dbContextFixture._dbContext;
        _sut = new SchoolsRepository(_dbContext);
    }

    [Fact]
    public async Task CreateSchool_ValidInput_Success()
    {
        var school = ValidSchoolNew(_fixture);

        var output = await _sut.CreateSchool(school);

        Assert.True(output > 0);
        Assert.Equal(output, school.Id);
    }

    [Fact]
    public async Task DeleteSchool_ValidInput_Success()
    {
        var school = ValidSchoolNew(_fixture);

        await _sut.CreateSchool(school);

        school = await _sut.GetSchool(school.Id);

        Assert.NotNull(school);

        var output = await _sut.DeleteSchool(school);

        Assert.True(output);

        school = await _sut.GetSchool(school.Id);

        Assert.Null(school);
    }

    [Fact]
    public async Task GetSchool_ValidInput_Success()
    {
        var school = ValidSchoolNew(_fixture);
        await _sut.CreateSchool(school);
        var output = await _sut.GetSchool(school.Id);

        Assert.NotNull(output);

        Assert.True(AreSchoolsEqual(school, output));
    }

    [Fact]
    public async Task SearchSchoolsByName_ValidInputEmptyName_Success()
    {
        var schools = Enumerable.Range(1, 10).Select(s => ValidSchoolNew(_fixture)).ToList();
        foreach (var school in schools)
        {
            await _sut.CreateSchool(school);
        }

        var output = await _sut.SearchSchoolsByName("", 1, 100);
        Assert.NotNull(output);
        Assert.True(output.TotalCount >= 10);
        Assert.True(output.Data.Contains(schools[0]));
    }

    [Fact]
    public async Task SearchSchoolsByName_ValidInput_Success()
    {
        var schools = Enumerable.Range(1, 10).Select(s => ValidSchoolNew(_fixture)).ToList();
        foreach (var school in schools)
        {
            await _sut.CreateSchool(school);
        }

        var output = await _sut.SearchSchoolsByName(schools[0].Name, 1, 10);
        Assert.NotNull(output);
        Assert.True(output.TotalCount >= 1);
        Assert.True(output.Data.Contains(schools[0]));
    }

    [Fact]
    public async Task UpdateClass_ValidInput_Success()
    {
        var school = ValidSchoolNew(_fixture);
        var classRoom = ValidClassNew(_fixture, 0);
        school.Classes.Add(classRoom);

        await _sut.CreateSchool(school);

        school.SetClassName(classRoom.Id, _fixture.Create<string>());

        var output = await _sut.UpdateClass(classRoom);

        Assert.True(output);
    }

    [Fact]
    public async Task UpdateClass_ValidInputNoChanges_Success()
    {
        var school = ValidSchoolNew(_fixture);
        var classRoom = ValidClassNew(_fixture, 0);
        school.Classes.Add(classRoom);

        await _sut.CreateSchool(school);

        var output = await _sut.UpdateClass(classRoom);

        Assert.False(output);
    }

    [Fact]
    public async Task UpdateSchool_ValidInput_Success()
    {
        var school = ValidSchoolNew(_fixture);
        await _sut.CreateSchool(school);

        school.SetName(_fixture.Create<string>());

        var output = await _sut.UpdateSchool(school);

        Assert.True(output);
    }

    [Fact]
    public async Task UpdateSchool_ValidInputClassAdded_Success()
    {
        var school = ValidSchoolNew(_fixture);
        var classRoom = ValidClassNew(_fixture, 0);

        await _sut.CreateSchool(school);
        school.Classes.Add(classRoom);


        var output = await _sut.UpdateSchool(school);

        Assert.True(output);
    }

    [Fact]
    public async Task UpdateSchool_ValidInputNoChanges_Success()
    {
        var school = ValidSchoolNew(_fixture);
        await _sut.CreateSchool(school);

        var output = await _sut.UpdateSchool(school);

        Assert.False(output);
    }

    private static bool AreSchoolsEqual(School school1, School school2)
    {
        return school1.Id == school2.Id &&
            school1.Name == school2.Name &&
            school1.OpenDate == school2.OpenDate;
    }

    private static School ValidSchoolNew(Fixture fixture) =>
        School.Create(null, fixture.Create<string>(), fixture.Create<DateTime>());

    private static ClassRoom ValidClassNew(Fixture fixture, int schoolId) =>
        ClassRoom.Create(null, fixture.Create<string>(), schoolId);
}
