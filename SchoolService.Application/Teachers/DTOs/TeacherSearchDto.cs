using SchoolService.Domain.Shared.Pagination;

namespace SchoolService.Application.Teachers.DTOs;
public class TeacherSearchDto : PagedRequerst
{
    public string? Name { get; set; }
    public int? SchoolId { get; set; }
    public int? ClassroomId { get; set; }
}
