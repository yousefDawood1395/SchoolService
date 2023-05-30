using SchoolService.Domain.Shared.Pagination;

namespace SchoolService.Application.Students.DTOs;

public class StudentSearchDto : PagedRequerst
{
    public string? Name { get; set; }
    public int? ClassRoomId { get; set; }
    public int? SchoolId { get; set; }
}
