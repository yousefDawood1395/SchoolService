using SchoolService.Application.Students.DTOs;
using SchoolService.Domain.DomainServices;
using SchoolService.Domain.Shared.Repositories;
using SchoolService.Domain.Shared.Validation;
using SchoolService.Domain.Students;

namespace SchoolService.Application.Students;
public class StudentsService : IStudentsService
{
    private readonly IStudentsRepository _studentsRepository;
    private readonly IStudentsDomainService _studentsDomainService;
    private readonly ISchoolsRepository _schoolRepository;
    private readonly IValidationEngine _validationEngine;

    public StudentsService(IStudentsRepository studentsRepository,
        IStudentsDomainService studentsDomainService,
        ISchoolsRepository schoolRepository,
        IValidationEngine validationEngine)
    {
        _studentsRepository = studentsRepository;
        _studentsDomainService = studentsDomainService;
        _schoolRepository = schoolRepository;
        _validationEngine = validationEngine;
    }

    public async Task<StudentDto> Create(StudentCreateDto input)
    {
        var student = input.ToStudent();

        await student.Create(_studentsRepository, _validationEngine);

        return StudentDto.FromStudent(student);
    }

    public async Task<StudentDto> Update(StudentUpdateDto input)
    {
        var student = await Student.Get(input.Id, _studentsRepository);
        student.SetName(input.Name);
        await student.Update(_studentsRepository, _validationEngine);
        return StudentDto.FromStudent(student);
    }

    public async Task<bool> SetClass(StudentSetClassroomDto input)
    {
        return await _studentsDomainService.SetClass(input.StudentId, input.SchoolId, input.ClassRoomId);
    }

    public async Task<bool> Delete(int id)
    {
        var student = await Student.Get(id, _studentsRepository);
        return await student.Delete(_studentsRepository);
    }
}
