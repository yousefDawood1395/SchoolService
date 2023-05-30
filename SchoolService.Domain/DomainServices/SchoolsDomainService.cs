using SchoolService.Domain.Schools;
using SchoolService.Domain.Shared.Exceptions;
using SchoolService.Domain.Shared.Repositories;
using SchoolService.Domain.Shared.Validation;
using SchoolService.Domain.Students;
using SchoolService.Domain.Teachers;

namespace SchoolService.Domain.DomainServices;
public class SchoolsDomainService : ISchoolsDomainService
{
    private readonly ISchoolsRepository _schoolRepository;
    private readonly ITeachersRepository _teachersRepository;
    private readonly IStudentsRepository _studentsRepository;
    public SchoolsDomainService(ISchoolsRepository schoolRepository, ITeachersRepository teachersRepository, IStudentsRepository studentsRepository)
    {
        _schoolRepository = schoolRepository;
        _teachersRepository = teachersRepository;
        _studentsRepository = studentsRepository;
    }

    public async Task<bool> DeleteSchool(int id)
    {
        School school = await School.Get(id, _schoolRepository);
        if (school.Classes.Any())
        {
            throw new BusinessException("School has classes");
        }

        var schoolTeachers = await Teacher.Search(null, id, null, 1, 1, _teachersRepository);
        if (schoolTeachers.TotalCount > 0)
        {
            throw new BusinessException("School has teachers");
        }

        var schoolStudents = await Student.Search(_studentsRepository, null, id, null, 1, 1);

        if (schoolStudents.TotalCount > 0)
        {
            throw new BusinessException("School has students");
        }

        return await school.Delete(_schoolRepository);
    }

    public async Task<bool> DeleteClass(int schoolId, int classId)
    {
        var dbSchool = await School.Get(schoolId, _schoolRepository);
        var classTeachers = await Teacher.Search(null, null, classId, 1, 1, _teachersRepository);
        if (classTeachers.TotalCount > 0)
        {
            throw new BusinessException("Class has teachers");
        }

        var classStudents = await Student.Search(_studentsRepository, null, null, classId, 1, 1);
        if (classStudents.TotalCount > 0)
        {
            throw new BusinessException("Class has students");
        }

        return await dbSchool.DeleteClass(classId, _schoolRepository);
    }
}