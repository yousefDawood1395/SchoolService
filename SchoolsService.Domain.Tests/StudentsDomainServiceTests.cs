using AutoFixture;
using FluentValidation.Results;
using Moq;
using SchoolService.Domain.DomainServices;
using SchoolService.Domain.Schools;
using SchoolService.Domain.Shared.Exceptions;
using SchoolService.Domain.Shared.Pagination;
using SchoolService.Domain.Shared.Repositories;
using SchoolService.Domain.Shared.Validation;
using SchoolService.Domain.Students;

namespace SchoolsService.Domain.Tests;
public class StudentsDomainServiceTests
{
    private readonly Mock<IStudentsRepository> _studentsRepository;
    private readonly Mock<ISchoolsRepository> _schoolsRepository;
    private readonly IStudentsDomainService _sut;
    private readonly Mock<IValidationEngine> _validationEngine;
    private readonly Fixture _fixture;
    public StudentsDomainServiceTests()
    {
        _studentsRepository = new Mock<IStudentsRepository>();
        _schoolsRepository = new Mock<ISchoolsRepository>();
        _validationEngine = new Mock<IValidationEngine>();
        _fixture = new Fixture();
        _sut = new StudentsDomainService(_studentsRepository.Object, _schoolsRepository.Object, _validationEngine.Object);

        _validationEngine.Setup(x => x.Validate(It.IsAny<Student>(), true)).Returns(new List<ValidationFailure>());
    }

    [Fact]
    public async Task SetClass_ValidInput_Success()
    {
        var school = ValidSchool(_fixture);
        var classroom = ValidClassroom(_fixture);
        school.Classes.Add(classroom);

        var student = ValidStudent(_fixture);

        _schoolsRepository.Setup(x => x.GetSchool(It.IsAny<int>())).ReturnsAsync(school);
        _studentsRepository.Setup(x => x.Get(It.IsAny<int>())).ReturnsAsync(student);
        _studentsRepository.Setup(x => x.Update(It.IsAny<Student>())).ReturnsAsync(true);

        var output = await _sut.SetClass(student.Id, school.Id, classroom.Id);
    }

    [Fact]
    public async Task SetClass_InvalidInput_NoClass_ThrowsNotFoundexception()
    {
        var school = ValidSchool(_fixture);

        var student = ValidStudent(_fixture);

        _schoolsRepository.Setup(x => x.GetSchool(It.IsAny<int>())).ReturnsAsync(school);
        _studentsRepository.Setup(x => x.Get(It.IsAny<int>())).ReturnsAsync(student);
        _studentsRepository.Setup(x => x.Update(It.IsAny<Student>())).ReturnsAsync(true);

        await Assert.ThrowsAsync<DataNotFoundException>(() => _sut.SetClass(student.Id, school.Id, It.IsAny<int>()));
    }



    private static School ValidSchool(Fixture fixture) =>
   School.Create(fixture.Create<int>(), fixture.Create<string>(), fixture.Create<DateTime>());

    private static ClassRoom ValidClassroom(Fixture fixture) =>
        ClassRoom.Create(fixture.Create<int>(), fixture.Create<string>(), fixture.Create<int>());

    private static Student ValidStudent(Fixture fixtur) =>
        Student.Create(fixtur.Create<int>(), fixtur.Create<string>(), fixtur.Create<int>(), fixtur.Create<int>());
}
