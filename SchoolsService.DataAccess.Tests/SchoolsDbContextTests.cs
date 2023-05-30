using AutoFixture;
using SchoolService.DataAccess.Tests.SchoolsDbContextFixtures;
using SchoolService.Domain.Schools;
using SchoolService.Domain.Students;
using SchoolService.Domain.Teachers;

namespace SchoolService.DataAccess.Tests;
[Collection(nameof(SchoolsDbContextFixtureCollectionDefinition))]
public class SchoolsDbContextTests
{
    private readonly SchoolsDbContext _dbContext;
    private readonly Fixture _fixture;
    public SchoolsDbContextTests(SchoolsDbContextFixture dbContextFixture)
    {
        _dbContext = dbContextFixture._dbContext;
        _fixture = new Fixture();
    }

    [Fact]
    public async Task
    private static School ValidSchoolNew(Fixture fixture) =>
    School.Create(null, fixture.Create<string>(), fixture.Create<DateTime>());

    private static ClassRoom ValidClassNew(Fixture fixture, int schoolId) =>
        ClassRoom.Create(null, fixture.Create<string>(), schoolId);

    private static Student ValidStudentNew(Fixture fixture) =>
    Student.Create(null, fixture.Create<string>(), fixture.Create<int>(), fixture.Create<int>());

    private static Teacher ValidTeacherNew(Fixture fixture) =>
    Teacher.Create(null, fixture.Create<string>());

    private static TeacherClassrooms ValidTeacherClassroomNew(Fixture fixture) =>
        TeacherClassrooms.Create(null, 0, fixture.Create<int>(), fixture.Create<int>());
}
