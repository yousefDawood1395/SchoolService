using SchoolService.Application.Students.DTOs;
using SchoolService.Domain.Shared.Pagination;
using SchoolService.Domain.Shared.Repositories;
using SchoolService.Domain.Students;
using System.Runtime.CompilerServices;

namespace SchoolService.Application.Students;
public class StudentsQueryService : IStudentsQueryService
{
    private readonly IStudentsRepository _studentsRepository;
    public StudentsQueryService(IStudentsRepository studentsRepository)
    {
        _studentsRepository = studentsRepository;
    }

    public async Task<StudentDto> Get(int id)
    {
        var student = await Student.Get(id, _studentsRepository);
        return StudentDto.FromStudent(student);
    }
    public async Task<PagedResponse<StudentDto>> Search(StudentSearchDto input)
    {
        var output = await Student.Search(_studentsRepository, input.Name, input.SchoolId,
            input.ClassRoomId, input.PageNo, input.PageSize);

        return new PagedResponse<StudentDto>
        {
            PageNumber = output.PageNumber,
            PageSize = output.PageSize,
            TotalCount = output.TotalCount,
            Data = output.Data.Select(s => StudentDto.FromStudent(s)).ToList()
        };
    }
}