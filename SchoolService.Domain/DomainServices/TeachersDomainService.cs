using SchoolService.Domain.Schools;
using SchoolService.Domain.Shared.Exceptions;
using SchoolService.Domain.Shared.Repositories;
using SchoolService.Domain.Teachers;

namespace SchoolService.Domain.DomainServices;
public class TeachersDomainService : ITeachersDomainService
{
    private readonly ITeachersRepository _teachersRepository;
    private readonly ISchoolsRepository _schoolRepository;

    public TeachersDomainService(ITeachersRepository teachersRepository, ISchoolsRepository schoolRepository)
    {
        _teachersRepository = teachersRepository;
        _schoolRepository = schoolRepository;
    }

    public async Task<bool> AssignTeacherToClass(int teacherId, int classroomId, int schoolId)
    {
        var dbTeacher = await Teacher.Get(teacherId, _teachersRepository);

        var dbSchool = await School.Get(schoolId, _schoolRepository);

        if (!dbSchool.Classes.Any(c => c.Id == classroomId))
        {
            throw new DataNotFoundException("Class doesn't found");
        }

        return await dbTeacher.AssignTeacherToClass(schoolId, classroomId, _teachersRepository);
    }

    public async Task<bool> UnAssignTeacherFromClass(int teacherId, int classroomId)
    {
        var dbTeacher = await Teacher.Get(teacherId, _teachersRepository);

        if (!dbTeacher.Classrooms.Any(a => a.ClassroomId == classroomId))
        {
            throw new DataNotFoundException("Teacher is not assigned to this class");
        }

        return await dbTeacher.UnAssignTecherToClass(classroomId, _teachersRepository);
    }
}