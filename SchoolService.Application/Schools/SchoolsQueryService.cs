using SchoolService.Application.Schools.Dtos;
using SchoolService.Domain.Schools;
using SchoolService.Domain.Shared.Pagination;
using SchoolService.Domain.Shared.Repositories;

namespace SchoolService.Application.Schools;
public class SchoolsQueryService : ISchoolsQueryService
{
    private readonly ISchoolsRepository _schoolRepository;

    public SchoolsQueryService(ISchoolsRepository schoolRepository)
    {
        _schoolRepository = schoolRepository;
    }

    public async Task<SchoolDto> GetSchool(int id)
    {
        var output = await School.Get(id, _schoolRepository);
        return SchoolDto.FromSchool(output);
    }

    public async Task<PagedResponse<SchoolDto>> SearchSchools(SchoolSearchDto input)
    {
        var output = await School.SearchSchools(input.Name, input.PageNo, input.PageSize, _schoolRepository);

        return new PagedResponse<SchoolDto>
        {
            PageNumber = output.PageNumber,
            TotalCount = output.TotalCount,
            PageSize = output.PageSize,
            Data = output.Data.Select(s => SchoolDto.FromSchool(s)).ToList()
        };
    }
}