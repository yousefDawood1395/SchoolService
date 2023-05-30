using SchoolService.Domain.Shared.Pagination;

namespace SchoolService.Application.Schools.Dtos;
public class SchoolSearchDto : PagedRequerst
{
    public string? Name { get; set; }
}