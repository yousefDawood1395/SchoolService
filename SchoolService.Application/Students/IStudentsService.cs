using SchoolService.Application.Students.DTOs;

namespace SchoolService.Application.Students;
public interface IStudentsService
{
    Task<StudentDto> Create(StudentCreateDto input);
    Task<bool> Delete(int id);
    Task<bool> SetClass(StudentSetClassroomDto input);
    Task<StudentDto> Update(StudentUpdateDto input);
}