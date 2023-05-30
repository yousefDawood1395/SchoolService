using AutoFixture;
using SchoolService.Domain.Schools;
using SchoolService.Domain.Shared.Repositories;
using SchoolService.Domain.Students;
using SchoolService.Infrastructure.Repositories;
using SchoolService.Infrastructure.Tests.SchoolsDbContextFixtures;

namespace SchoolService.Infrastructure.Tests;
[Collection(nameof(SchoolsDbContextFixtureCollectionDefinition))]
public class StudentsRepositoryTests
{
    private readonly IStudentsRepository _sut;
    private readonly Fixture _fixture;
    public StudentsRepositoryTests(SchoolsDbContextFixture dbContextFixture)
    {
        _sut = new StudentsRepository(dbContextFixture._dbContext);
        _fixture = new Fixture();
    }

    [Fact]
    public async Task Create_ValidInput_Success()
    {
        var student = ValidStudentNew(_fixture);
        var output = await _sut.Create(student);

        Assert.Equal(student.Id, output);
    }

    [Fact]
    public async Task Delete_ValidInput_Success()
    {
        var student = ValidStudentNew(_fixture);
        await _sut.Create(student);
        var output = await _sut.Delete(student);

        Assert.True(output);
    }

    [Fact]
    public async Task Get_ValidInput_Success()
    {
        var student = ValidStudentNew(_fixture);
        await _sut.Create(student);
        var output = await _sut.Get(student.Id);

        Assert.NotNull(output);

        Assert.True(AreStudentsEqual(student, output));
    }

    [Fact]
    public async Task Search_ValidInput_Success()
    {
        int schooldId = _fixture.Create<int>();
        int classroomId = _fixture.Create<int>();
        var students = Enumerable.Range(1, 10).Select(s => ValidStudentNewFixedSchoolAndClass(_fixture, schooldId, classroomId)).ToList();


        foreach (var item in students)
        {
            await _sut.Create(item);
        }

        var output = await _sut.Search(null, schooldId, classroomId, 1, 10);

        Assert.NotNull(output);

        Assert.True(output.TotalCount >= 10);

        Assert.Contains(students[0], output.Data);
    }

    [Fact]
    public async Task Search_ValidInputByName_Success()
    {
        var student = ValidStudentNew(_fixture);

        await _sut.Create(student);

        var output = await _sut.Search(student.Name, null, null, 1, 10);

        Assert.NotNull(output);

        Assert.True(output.TotalCount >= 1);

        Assert.Contains(student, output.Data);
    }

    [Fact]
    public async Task Update_ValidInput_Success()
    {
        var student = ValidStudentNew(_fixture);

        await _sut.Create(student);

        student.SetName(_fixture.Create<string>());

        var output = await _sut.Update(student);

        Assert.True(output);
    }

    [Fact]
    public async Task Update_ValidInputNoChanges_Success()
    {
        var student = ValidStudentNew(_fixture);

        await _sut.Create(student);

        var output = await _sut.Update(student);

        Assert.False(output);
    }
    private static bool AreStudentsEqual(Student student1, Student student2)
    {
        return student1.Id == student2.Id &&
            student1.Name == student2.Name &&
            student1.SchoolId == student2.SchoolId &&
            student1.ClassRoomId == student2.ClassRoomId;
    }

    private static Student ValidStudentNew(Fixture fixture) =>
        Student.Create(null, fixture.Create<string>(), fixture.Create<int>(), fixture.Create<int>());

    private static Student ValidStudentNewFixedSchoolAndClass(Fixture fixture, int? schooldId, int? classroomId) =>
        Student.Create(null, fixture.Create<string>(), classroomId, schooldId);
}
