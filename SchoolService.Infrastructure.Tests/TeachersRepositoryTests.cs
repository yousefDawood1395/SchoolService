using AutoFixture;
using SchoolService.Domain.Shared.Repositories;
using SchoolService.Domain.Teachers;
using SchoolService.Infrastructure.Repositories;
using SchoolService.Infrastructure.Tests.SchoolsDbContextFixtures;

namespace SchoolService.Infrastructure.Tests;
[Collection(nameof(SchoolsDbContextFixtureCollectionDefinition))]
public class TeachersRepositoryTests
{
    private readonly ITeachersRepository _sut;
    private readonly Fixture _fixture;

    public TeachersRepositoryTests(SchoolsDbContextFixture dbContextFixture)
    {
        _sut = new TeachersRepository(dbContextFixture._dbContext);
        _fixture = new Fixture();
    }

    [Fact]
    public async Task Create_ValidInput_Success()
    {
        var teacher = ValidTeacherNew(_fixture);

        var output = await _sut.Create(teacher);

        Assert.Equal(output, teacher.Id);
    }

    [Fact]
    public async Task Delete_ValidInput_Success()
    {
        var teacher = ValidTeacherNew(_fixture);

        await _sut.Create(teacher);

        var output = await _sut.Delete(teacher);

        Assert.True(output);
    }

    [Fact]
    public async Task Get_ValidInput_Success()
    {
        var teacher = ValidTeacherNew(_fixture);
        teacher.Classrooms.Add(ValidTeacherClassroomNew(_fixture));

        await _sut.Create(teacher);

        var output = await _sut.Get(teacher.Id);

        Assert.NotNull(output);

        Assert.Equal(teacher, output);
    }

    [Fact]
    public async Task GetRow_ValidInput_Success()
    {
        var teacher = ValidTeacherNew(_fixture);

        await _sut.Create(teacher);

        var output = await _sut.GetRow(teacher.Id);

        Assert.NotNull(output);

        Assert.Equal(teacher, output);
    }

    [Fact]
    public async Task Search_ValidInput_Success()
    {
        var teachers = Enumerable.Range(1, 10).Select(s => ValidTeacherNew(_fixture)).ToList();

        var schoolId = _fixture.Create<int>();
        var classroomId = _fixture.Create<int>();

        foreach (var item in teachers)
        {
            item.Classrooms.Add(TeacherClassrooms.Create(null, 0, classroomId, schoolId));
            await _sut.Create(item);
        }

        var output = await _sut.Search(null, schoolId, classroomId, 1, 10);

        Assert.NotNull(output);
        Assert.True(output.TotalCount >= 10);

        Assert.Contains(teachers[0], output.Data);
    }

    [Fact]
    public async Task Search_ValidInputByName_Success()
    {
        var teachers = Enumerable.Range(1, 10).Select(s => ValidTeacherNew(_fixture)).ToList();

        foreach (var item in teachers)
        {
            await _sut.Create(item);
        }

        var output = await _sut.Search(teachers[0].Name, null, null, 1, 10);

        Assert.NotNull(output);
        Assert.True(output.TotalCount >= 1);

        Assert.Contains(teachers[0], output.Data);
    }

    [Fact]
    public async Task Update_ValidInput_Success()
    {
        var teacher = ValidTeacherNew(_fixture);

        await _sut.Create(teacher);

        teacher.SetName(_fixture.Create<string>());

        var output = await _sut.Update(teacher);

        Assert.True(output);
    }

    [Fact]
    public async Task Update_ValidInputClassrooms_Success()
    {
        var teacher = ValidTeacherNew(_fixture);

        await _sut.Create(teacher);

        teacher.Classrooms.Add(ValidTeacherClassroomNew(_fixture));

        var output = await _sut.Update(teacher);

        Assert.True(output);
    }

    [Fact]
    public async Task Update_ValidInputNoChanges_Success()
    {
        var teacher = ValidTeacherNew(_fixture);

        await _sut.Create(teacher);

        var output = await _sut.Update(teacher);

        Assert.False(output);
    }

    private static Teacher ValidTeacherNew(Fixture fixture) =>
        Teacher.Create(null, fixture.Create<string>());

    private static TeacherClassrooms ValidTeacherClassroomNew(Fixture fixture) =>
        TeacherClassrooms.Create(null, 0, fixture.Create<int>(), fixture.Create<int>());
}