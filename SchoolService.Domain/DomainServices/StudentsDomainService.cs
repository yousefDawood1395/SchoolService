using SchoolService.Domain.Schools;
using SchoolService.Domain.Shared.Exceptions;
using SchoolService.Domain.Shared.Repositories;
using SchoolService.Domain.Shared.Validation;
using SchoolService.Domain.Students;

namespace SchoolService.Domain.DomainServices;
public class StudentsDomainService : IStudentsDomainService
{
    private readonly IStudentsRepository _studentsRepository;
    private readonly ISchoolsRepository _schoolRepository;
    private readonly IValidationEngine _validationEngine;

    public StudentsDomainService(IStudentsRepository studentsRepository, ISchoolsRepository schoolRepository, IValidationEngine validationEngine)
    {
        _studentsRepository = studentsRepository;
        _schoolRepository = schoolRepository;
        _validationEngine = validationEngine;
    }

    public async Task<bool> SetClass(int studentId, int? schoolId, int? classroomId)
    {
        var dbStudent = await Student.Get(studentId, _studentsRepository);

        if (schoolId is not null)
        {
            var dbSchool = await School.Get(schoolId.Value, _schoolRepository);
            if (classroomId is not null)
            {
                if (!dbSchool.Classes.Any(a => a.Id == classroomId.Value))
                {
                    throw new DataNotFoundException("Class is not found");
                }
            }
        }

        return await dbStudent.SetClass(classroomId, schoolId, _validationEngine, _studentsRepository);
    }
}