using SchoolService.Domain.Shared.Pagination;
using SchoolService.Domain.Students;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolService.Domain.Shared.Repositories;
public interface IStudentsRepository
{
    Task<int> Create(Student input);
    Task<bool> Update(Student input);
    Task<bool> Delete(Student input);
    Task<Student?> Get(int id);
    Task<PagedResponse<Student>> Search(string? name, int? schoolId, int? classroomId, int pageNumber, int pageSize);
}