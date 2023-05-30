using SchoolService.Domain.Teachers;

namespace SchoolService.Application.Teachers.DTOs;
public class TeacherDto
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public IList<TeacherClassroomsDto> Classrooms { get; set; } = new List<TeacherClassroomsDto>();

    public static TeacherDto FromTeacher(Teacher input) =>
        new TeacherDto
        {
            Id = input.Id,
            Name = input.Name,
            Classrooms = input.Classrooms.Select(s => TeacherClassroomsDto.FromTeacherClassrooms(s)).ToList()
        };
}
