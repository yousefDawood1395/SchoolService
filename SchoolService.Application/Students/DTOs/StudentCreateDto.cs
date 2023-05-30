using SchoolService.Domain.Students;

namespace SchoolService.Application.Students.DTOs;
public class StudentCreateDto
{
    public string? Name { get; set; }
    public int? ClassRoomId { get; set; }
    public int? SchoolId { get; set; }

    public Student ToStudent() =>
        Student.Create(null, Name, ClassRoomId, SchoolId);
}
