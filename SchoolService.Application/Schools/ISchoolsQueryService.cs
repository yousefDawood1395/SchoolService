using SchoolService.Application.Schools.Dtos;
using SchoolService.Domain.Shared.Pagination;

namespace SchoolService.Application.Schools;
public interface ISchoolsQueryService
{
    Task<SchoolDto> GetSchool(int id);
    Task<PagedResponse<SchoolDto>> SearchSchools(SchoolSearchDto input);
}