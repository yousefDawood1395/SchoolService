using SchoolService.Domain.Teachers;

namespace SchoolService.Application.Teachers.DTOs;
public class TeacherClassroomsDto
{
    public int Id { get; set; }
    public int TeacherId { get; set; }
    public int ClassroomId { get; set; }
    public int SchoolId { get; set; }

    public static TeacherClassroomsDto FromTeacherClassrooms(TeacherClassrooms input) =>
        new TeacherClassroomsDto
        {
            Id = input.Id,
            TeacherId = input.TeacherId,
            ClassroomId = input.ClassroomId,
            SchoolId = input.SchoolId
        };

}
