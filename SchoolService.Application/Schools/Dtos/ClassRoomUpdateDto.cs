namespace SchoolService.Application.Schools.Dtos;
public class ClassRoomUpdateDto
{
    public int ClassRoomId { get; set; }
    public int SchoolId { get; set; }
    public string? Name { get; set; }
}