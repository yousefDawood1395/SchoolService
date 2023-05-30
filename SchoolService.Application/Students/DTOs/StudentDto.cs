using SchoolService.Domain.Students;

namespace SchoolService.Application.Students.DTOs;
public class StudentDto
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public int? ClassRoomId { get; set; }
    public int? SchoolId { get; set; }
    public static StudentDto FromStudent(Student student) =>
    new StudentDto
    {
        Id = student.Id,
        Name = student.Name,
        ClassRoomId = student.ClassRoomId,
        SchoolId = student.SchoolId
    };
}
