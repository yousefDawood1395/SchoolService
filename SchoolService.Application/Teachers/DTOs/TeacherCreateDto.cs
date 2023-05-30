using SchoolService.Domain.Teachers;

namespace SchoolService.Application.Teachers.DTOs;
public class TeacherCreateDto
{
    public string? Name { get; set; }
    public Teacher ToTeacher() => Teacher.Create(null, Name);
}
