using SchoolService.Application.Teachers.DTOs;

namespace SchoolService.Application.Teachers;
public interface ITeachersService
{
    Task<bool> AssignTeacherToClassroom(TeacherAssignClassroomDto input);
    Task<TeacherDto> Create(TeacherCreateDto input);
    Task<bool> Delete(int id);
    Task<bool> UnAssignTeacherToClassroom(TeacherUnAssignClassroomDto input);
    Task<TeacherDto> Update(TeacherUpdateDto input);
}