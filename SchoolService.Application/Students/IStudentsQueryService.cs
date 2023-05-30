using SchoolService.Application.Students.DTOs;
using SchoolService.Domain.Shared.Pagination;

namespace SchoolService.Application.Students;
public interface IStudentsQueryService
{
    Task<StudentDto> Get(int id);
    Task<PagedResponse<StudentDto>> Search(StudentSearchDto input);
}