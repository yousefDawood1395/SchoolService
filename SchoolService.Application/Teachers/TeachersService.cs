using SchoolService.Application.Teachers.DTOs;
using SchoolService.Domain.DomainServices;
using SchoolService.Domain.Shared.Repositories;
using SchoolService.Domain.Shared.Validation;
using SchoolService.Domain.Teachers;

namespace SchoolService.Application.Teachers;
public class TeachersService : ITeachersService
{
    private readonly ITeachersRepository _teachersRepository;
    private readonly IValidationEngine _validationEngine;
    private readonly ITeachersDomainService _teachersDomainService;

    public TeachersService(ITeachersRepository repository,
        IValidationEngine engine,
        ITeachersDomainService teachersDomainService)
    {
        _teachersRepository = repository;
        _validationEngine = engine;
        _teachersDomainService = teachersDomainService;
    }

    public async Task<TeacherDto> Create(TeacherCreateDto input)
    {
        var teacher = input.ToTeacher();
        await teacher.Create(_validationEngine, _teachersRepository);
        return TeacherDto.FromTeacher(teacher);
    }

    public async Task<TeacherDto> Update(TeacherUpdateDto input)
    {
        var teacher = await Teacher.Get(input.Id, _teachersRepository);
        teacher.SetName(input.Name);
        await teacher.Update(_validationEngine, _teachersRepository);
        return TeacherDto.FromTeacher(teacher);
    }

    public async Task<bool> Delete(int id)
    {
        return await Teacher.Delete(id, _teachersRepository);
    }
    public async Task<bool> AssignTeacherToClassroom(TeacherAssignClassroomDto input)
    {
        return await _teachersDomainService.AssignTeacherToClass(input.TeacherId, input.ClassroomId, input.SchoolId);
    }

    public async Task<bool> UnAssignTeacherToClassroom(TeacherUnAssignClassroomDto input)
    {
        return await _teachersDomainService.UnAssignTeacherFromClass(input.TeacherId, input.ClassroomId);
    }
}
