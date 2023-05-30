using AutoFixture;
using Moq;
using SchoolService.Domain.DomainServices;
using SchoolService.Domain.Schools;
using SchoolService.Domain.Shared.Exceptions;
using SchoolService.Domain.Shared.Repositories;
using SchoolService.Domain.Teachers;

namespace SchoolsService.Domain.Tests;
public class TeachersDomainServiceTests
{
    private readonly Mock<ITeachersRepository> _teachersRepository;
    private readonly Mock<ISchoolsRepository> _schoolsRepository;
    private readonly ITeachersDomainService _sut;
    private readonly Fixture _fixture;

    public TeachersDomainServiceTests()
    {
        _teachersRepository = new Mock<ITeachersRepository>();
        _schoolsRepository = new Mock<ISchoolsRepository>();
        _sut = new TeachersDomainService(_teachersRepository.Object, _schoolsRepository.Object);
        _fixture = new Fixture();
    }

    [Fact]
    public async Task AssignTeacherToClass_ValidInput_Success()
    {
        var teacher = ValidTeacher(_fixture);

        var school = ValidSchool(_fixture);
        var classroom = ValidClassroom(_fixture);

        school.Classes.Add(classroom);

        _schoolsRepository.Setup(x => x.GetSchool(It.IsAny<int>())).ReturnsAsync(school);
        _teachersRepository.Setup(x => x.Get(It.IsAny<int>())).ReturnsAsync(teacher);
        _teachersRepository.Setup(x => x.Update(It.IsAny<Teacher>())).ReturnsAsync(true);

        var output = await _sut.AssignTeacherToClass(teacher.Id, classroom.Id, school.Id);

        Assert.True(output);
    }

    [Fact]
    public async Task AssignTeacherToClass_InvalidInput_DuplicatedClass_ThrowsDuplicateException()
    {
        var teacher = ValidTeacher(_fixture);
        var teacherClass = ValidTeacherClassroom(_fixture);

        teacher.Classrooms.Add(teacherClass);

        var school = ValidSchool(_fixture);
        var classroom = ClassRoom.Create(teacherClass.ClassroomId, _fixture.Create<string>(), school.Id);

        school.Classes.Add(classroom);

        _schoolsRepository.Setup(x => x.GetSchool(It.IsAny<int>())).ReturnsAsync(school);
        _teachersRepository.Setup(x => x.Get(It.IsAny<int>())).ReturnsAsync(teacher);
        _teachersRepository.Setup(x => x.Update(It.IsAny<Teacher>())).ReturnsAsync(true);

        await Assert.ThrowsAsync<DataDuplicateException>(() => _sut.AssignTeacherToClass(teacher.Id, classroom.Id, school.Id));
    }


    [Fact]
    public async Task AssignTeacherToClass_InvalidInput_NoClassExists_ThrowsNotFoundException()
    {
        var teacher = ValidTeacher(_fixture);

        var school = ValidSchool(_fixture);
        var classroom = ValidClassroom(_fixture);

        school.Classes.Add(classroom);

        _schoolsRepository.Setup(x => x.GetSchool(It.IsAny<int>())).ReturnsAsync(school);
        _teachersRepository.Setup(x => x.Get(It.IsAny<int>())).ReturnsAsync(teacher);
        _teachersRepository.Setup(x => x.Update(It.IsAny<Teacher>())).ReturnsAsync(true);

        await Assert.ThrowsAsync<DataNotFoundException>(() => _sut.AssignTeacherToClass(teacher.Id, _fixture.Create<int>(), school.Id));
    }

    [Fact]
    public async Task UnAssignTeacherFromClass_ValidInput_Success()
    {
        var teacher = ValidTeacher(_fixture);
        var teacherClass = ValidTeacherClassroom(_fixture);

        teacher.Classrooms.Add(teacherClass);

        _teachersRepository.Setup(x => x.Get(It.IsAny<int>())).ReturnsAsync(teacher);
        _teachersRepository.Setup(x => x.Update(It.IsAny<Teacher>())).ReturnsAsync(true);

        var output = await _sut.UnAssignTeacherFromClass(teacher.Id, teacherClass.ClassroomId);
    }

    [Fact]
    public async Task UnAssignTeacherFromClass_InvalidInput_ThrowsNotfoundException()
    {
        var teacher = ValidTeacher(_fixture);
        var teacherClass = ValidTeacherClassroom(_fixture);

        teacher.Classrooms.Add(teacherClass);

        _teachersRepository.Setup(x => x.Get(It.IsAny<int>())).ReturnsAsync(teacher);
        _teachersRepository.Setup(x => x.Update(It.IsAny<Teacher>())).ReturnsAsync(true);

        await Assert.ThrowsAsync<DataNotFoundException>(() => _sut.UnAssignTeacherFromClass(teacher.Id, _fixture.Create<int>()));
    }

    private static Teacher ValidTeacher(Fixture fixture) =>
       Teacher.Create(fixture.Create<int>(), fixture.Create<string>());

    private static TeacherClassrooms ValidTeacherClassroom(Fixture fixture) =>
        TeacherClassrooms.Create(fixture.Create<int>(), fixture.Create<int>(), fixture.Create<int>(), fixture.Create<int>());

    private static School ValidSchool(Fixture fixture) =>
        School.Create(fixture.Create<int>(), fixture.Create<string>(), fixture.Create<DateTime>());

    private static ClassRoom ValidClassroom(Fixture fixture) =>
        ClassRoom.Create(fixture.Create<int>(), fixture.Create<string>(), fixture.Create<int>());
}
