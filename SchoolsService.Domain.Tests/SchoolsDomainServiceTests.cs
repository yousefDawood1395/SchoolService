using AutoFixture;
using Moq;
using SchoolService.Domain.DomainServices;
using SchoolService.Domain.Schools;
using SchoolService.Domain.Shared.Exceptions;
using SchoolService.Domain.Shared.Pagination;
using SchoolService.Domain.Shared.Repositories;
using SchoolService.Domain.Students;
using SchoolService.Domain.Teachers;
using System.Runtime.CompilerServices;

namespace SchoolsService.Domain.Tests;
public class SchoolsDomainServiceTests
{
    private readonly Mock<ISchoolsRepository> _schoolsRepository;
    private readonly Mock<ITeachersRepository> _teachersRepository;
    private readonly Mock<IStudentsRepository> _studentsRepository;
    private readonly ISchoolsDomainService _sut;
    private readonly Fixture _fixture;

    public SchoolsDomainServiceTests()
    {
        _schoolsRepository = new Mock<ISchoolsRepository>();
        _teachersRepository = new Mock<ITeachersRepository>();
        _studentsRepository = new Mock<IStudentsRepository>();
        _sut = new SchoolsDomainService(_schoolsRepository.Object, _teachersRepository.Object, _studentsRepository.Object);
        _fixture = new Fixture();
    }

    [Fact]
    public async Task Delete_ValidInput_Success()
    {
        _schoolsRepository.Setup(x => x.GetSchool(It.IsAny<int>())).ReturnsAsync(ValidSchool(_fixture));
        _schoolsRepository.Setup(x => x.DeleteSchool(It.IsAny<School>())).ReturnsAsync(true);
        _teachersRepository.Setup(x => x.Search(It.IsAny<string?>(), It.IsAny<int?>(), It.IsAny<int?>(), It.IsAny<int>(), It.IsAny<int>()))
            .ReturnsAsync(PagedTeachersEmpty());
        _studentsRepository.Setup(x => x.Search(It.IsAny<string?>(), It.IsAny<int?>(), It.IsAny<int?>(), It.IsAny<int>(), It.IsAny<int>()))
            .ReturnsAsync(PagedStudentsEmpty());

        var outpuit = await _sut.DeleteSchool(_fixture.Create<int>());

        Assert.True(outpuit);
    }

    [Fact]
    public async Task Delete_InvvalidInput_ClassesExist_ThrowsBusinessException()
    {
        var school = ValidSchool(_fixture);
        school.Classes.Add(ValidClassroom(_fixture));

        _schoolsRepository.Setup(x => x.GetSchool(It.IsAny<int>())).ReturnsAsync(school);
        _schoolsRepository.Setup(x => x.DeleteSchool(It.IsAny<School>())).ReturnsAsync(true);
        _teachersRepository.Setup(x => x.Search(It.IsAny<string?>(), It.IsAny<int?>(), It.IsAny<int?>(), It.IsAny<int>(), It.IsAny<int>()))
            .ReturnsAsync(PagedTeachersEmpty());
        _studentsRepository.Setup(x => x.Search(It.IsAny<string?>(), It.IsAny<int?>(), It.IsAny<int?>(), It.IsAny<int>(), It.IsAny<int>()))
            .ReturnsAsync(PagedStudentsEmpty());

        await Assert.ThrowsAsync<BusinessException>(() => _sut.DeleteSchool(_fixture.Create<int>()));
    }

    [Fact]
    public async Task Delete_InvvalidInput_TeachersExist_ThrowsBusinessException()
    {
        _schoolsRepository.Setup(x => x.GetSchool(It.IsAny<int>())).ReturnsAsync(ValidSchool(_fixture));
        _schoolsRepository.Setup(x => x.DeleteSchool(It.IsAny<School>())).ReturnsAsync(true);
        _teachersRepository.Setup(x => x.Search(It.IsAny<string?>(), It.IsAny<int?>(), It.IsAny<int?>(), It.IsAny<int>(), It.IsAny<int>()))
            .ReturnsAsync(PagedTeachers(_fixture));
        _studentsRepository.Setup(x => x.Search(It.IsAny<string?>(), It.IsAny<int?>(), It.IsAny<int?>(), It.IsAny<int>(), It.IsAny<int>()))
            .ReturnsAsync(PagedStudentsEmpty());

        await Assert.ThrowsAsync<BusinessException>(() => _sut.DeleteSchool(_fixture.Create<int>()));
    }

    [Fact]
    public async Task Delete_InvvalidInput_StudentsExist_ThrowsBusinessException()
    {
        _schoolsRepository.Setup(x => x.GetSchool(It.IsAny<int>())).ReturnsAsync(ValidSchool(_fixture));
        _schoolsRepository.Setup(x => x.DeleteSchool(It.IsAny<School>())).ReturnsAsync(true);
        _teachersRepository.Setup(x => x.Search(It.IsAny<string?>(), It.IsAny<int?>(), It.IsAny<int?>(), It.IsAny<int>(), It.IsAny<int>()))
            .ReturnsAsync(PagedTeachersEmpty());
        _studentsRepository.Setup(x => x.Search(It.IsAny<string?>(), It.IsAny<int?>(), It.IsAny<int?>(), It.IsAny<int>(), It.IsAny<int>()))
            .ReturnsAsync(PagedStudents(_fixture));

        await Assert.ThrowsAsync<BusinessException>(() => _sut.DeleteSchool(_fixture.Create<int>()));
    }

    [Fact]
    public async Task DeleteClass_ValidInout_Success()
    {
        var school = ValidSchool(_fixture);
        var classroom = ValidClassroom(_fixture);
        school.Classes.Add(classroom);

        _schoolsRepository.Setup(x => x.GetSchool(It.IsAny<int>())).ReturnsAsync(school);
        _schoolsRepository.Setup(x => x.UpdateSchool(It.IsAny<School>())).ReturnsAsync(true);

        _teachersRepository.Setup(x => x.Search(It.IsAny<string?>(), It.IsAny<int?>(), It.IsAny<int?>(), It.IsAny<int>(), It.IsAny<int>()))
            .ReturnsAsync(PagedTeachersEmpty());

        _studentsRepository.Setup(x => x.Search(It.IsAny<string?>(), It.IsAny<int?>(), It.IsAny<int?>(), It.IsAny<int>(), It.IsAny<int>()))
            .ReturnsAsync(PagedStudentsEmpty());

        var output = await _sut.DeleteClass(school.Id, classroom.Id);

        Assert.True(output);
    }

    [Fact]
    public async Task DeleteClass_InvalidInput_NoClass_ThrowsNotFoundException()
    {
        var school = ValidSchool(_fixture);
        var classroom = ValidClassroom(_fixture);
        school.Classes.Add(classroom);

        _schoolsRepository.Setup(x => x.GetSchool(It.IsAny<int>())).ReturnsAsync(school);
        _schoolsRepository.Setup(x => x.UpdateSchool(It.IsAny<School>())).ReturnsAsync(true);

        _teachersRepository.Setup(x => x.Search(It.IsAny<string?>(), It.IsAny<int?>(), It.IsAny<int?>(), It.IsAny<int>(), It.IsAny<int>()))
            .ReturnsAsync(PagedTeachersEmpty());

        _studentsRepository.Setup(x => x.Search(It.IsAny<string?>(), It.IsAny<int?>(), It.IsAny<int?>(), It.IsAny<int>(), It.IsAny<int>()))
            .ReturnsAsync(PagedStudentsEmpty());

        await Assert.ThrowsAsync<DataNotFoundException>(() => _sut.DeleteClass(school.Id, _fixture.Create<int>()));
    }

    [Fact]
    public async Task DeleteClass_InvalidInput_TeachersExist_ThrowsBusinessException()
    {
        var school = ValidSchool(_fixture);
        var classroom = ValidClassroom(_fixture);
        school.Classes.Add(classroom);

        _schoolsRepository.Setup(x => x.GetSchool(It.IsAny<int>())).ReturnsAsync(school);
        _schoolsRepository.Setup(x => x.UpdateSchool(It.IsAny<School>())).ReturnsAsync(true);

        _teachersRepository.Setup(x => x.Search(It.IsAny<string?>(), It.IsAny<int?>(), It.IsAny<int?>(), It.IsAny<int>(), It.IsAny<int>()))
            .ReturnsAsync(PagedTeachers(_fixture));

        _studentsRepository.Setup(x => x.Search(It.IsAny<string?>(), It.IsAny<int?>(), It.IsAny<int?>(), It.IsAny<int>(), It.IsAny<int>()))
            .ReturnsAsync(PagedStudentsEmpty());

        await Assert.ThrowsAsync<BusinessException>(() => _sut.DeleteClass(school.Id, classroom.Id));
    }

    [Fact]
    public async Task DeleteClass_InvalidInput_StudentsExist_ThrowsBusinessException()
    {
        var school = ValidSchool(_fixture);
        var classroom = ValidClassroom(_fixture);
        school.Classes.Add(classroom);

        _schoolsRepository.Setup(x => x.GetSchool(It.IsAny<int>())).ReturnsAsync(school);
        _schoolsRepository.Setup(x => x.UpdateSchool(It.IsAny<School>())).ReturnsAsync(true);

        _teachersRepository.Setup(x => x.Search(It.IsAny<string?>(), It.IsAny<int?>(), It.IsAny<int?>(), It.IsAny<int>(), It.IsAny<int>()))
            .ReturnsAsync(PagedTeachersEmpty());

        _studentsRepository.Setup(x => x.Search(It.IsAny<string?>(), It.IsAny<int?>(), It.IsAny<int?>(), It.IsAny<int>(), It.IsAny<int>()))
            .ReturnsAsync(PagedStudents(_fixture));

        await Assert.ThrowsAsync<BusinessException>(() => _sut.DeleteClass(school.Id, classroom.Id));
    }

    private static School ValidSchool(Fixture fixture) =>
    School.Create(fixture.Create<int>(), fixture.Create<string>(), fixture.Create<DateTime>());

    private static ClassRoom ValidClassroom(Fixture fixture) =>
        ClassRoom.Create(fixture.Create<int>(), fixture.Create<string>(), fixture.Create<int>());

    private static Student ValidStudent(Fixture fixtur) =>
        Student.Create(fixtur.Create<int>(), fixtur.Create<string>(), fixtur.Create<int>(), fixtur.Create<int>());

    private static Teacher ValidTeacher(Fixture fixture) =>
        Teacher.Create(fixture.Create<int>(), fixture.Create<string>());

    private static PagedResponse<Student> PagedStudents(Fixture fixture) =>
        new PagedResponse<Student>
        {
            PageNumber = 1,
            PageSize = 10,
            TotalCount = 10,
            Data = Enumerable.Range(1, 10).Select(s => ValidStudent(fixture)).ToList()
        };

    private static PagedResponse<Student> PagedStudentsEmpty() =>
        new PagedResponse<Student>
        {
            PageNumber = 1,
            PageSize = 10,
            TotalCount = 0,
            Data = new List<Student>()
        };

    private static PagedResponse<Teacher> PagedTeachers(Fixture fixture) =>
        new PagedResponse<Teacher>
        {
            PageNumber = 1,
            PageSize = 10,
            TotalCount = 10,
            Data = Enumerable.Range(1, 10).Select(s => ValidTeacher(fixture)).ToList()
        };

    private static PagedResponse<Teacher> PagedTeachersEmpty() =>
        new PagedResponse<Teacher>
        {
            PageNumber = 1,
            PageSize = 10,
            TotalCount = 0,
            Data = new List<Teacher>()
        };
}
