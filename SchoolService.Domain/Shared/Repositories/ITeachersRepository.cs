using SchoolService.Domain.Shared.Pagination;
using SchoolService.Domain.Teachers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolService.Domain.Shared.Repositories;
public interface ITeachersRepository
{
    Task<int> Create(Teacher input);
    Task<bool> Update(Teacher input);
    Task<bool> Delete(Teacher input);
    Task<Teacher?> Get(int id);
    Task<Teacher?> GetRow(int id);
    Task<PagedResponse<Teacher>> Search(string? name, int? schoolId, int? classroomId, int pageNumber, int pageSize);
}
