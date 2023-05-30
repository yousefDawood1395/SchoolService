using SchoolService.Application.Teachers.DTOs;
using SchoolService.Domain.Shared.Pagination;

namespace SchoolService.Application.Teachers;
public interface ITeachersQueryService
{
    Task<TeacherDto> Get(int id);
    Task<PagedResponse<TeacherDto>> Search(TeacherSearchDto input);
}