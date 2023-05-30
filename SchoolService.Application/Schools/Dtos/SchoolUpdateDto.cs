using SchoolService.Domain.Schools;

namespace SchoolService.Application.Schools.Dtos;
public class SchoolUpdateDto
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public DateTime? OpenDate { get; set; }
}
