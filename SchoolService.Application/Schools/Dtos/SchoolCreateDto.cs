using SchoolService.Domain.Schools;

namespace SchoolService.Application.Schools.Dtos;

public class SchoolCreateDto
{
    public string? Name { get; set; }
    public DateTime? OpenDate { get; set; }
    public School ToSchool() => School.Create(null, Name, OpenDate);
}