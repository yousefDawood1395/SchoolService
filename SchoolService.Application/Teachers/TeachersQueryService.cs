using SchoolService.Application.Teachers.DTOs;
using SchoolService.Domain.Shared.Pagination;
using SchoolService.Domain.Shared.Repositories;
using SchoolService.Domain.Teachers;

namespace SchoolService.Application.Teachers;
public class TeachersQueryService : ITeachersQueryService
{
    private readonly ITeachersRepository _teachersRepository;

    public TeachersQueryService(ITeachersRepository teachersRepository)
    {
        _teachersRepository = teachersRepository;
    }

    public async Task<TeacherDto> Get(int id)
    {
        var teacher = await Teacher.Get(id, _teachersRepository);
        return TeacherDto.FromTeacher(teacher);
    }

    public async Task<PagedResponse<TeacherDto>> Search(TeacherSearchDto input)
    {
        var output = await Teacher.Search(input.Name, input.SchoolId, input.ClassroomId, input.PageNo, input.PageSize, _teachersRepository);
        return new PagedResponse<TeacherDto>
        {
            PageSize = output.PageSize,
            PageNumber = output.PageNumber,
            TotalCount = output.TotalCount,
            Data = output.Data.Select(s => TeacherDto.FromTeacher(s)).ToList()
        };
    }
}
