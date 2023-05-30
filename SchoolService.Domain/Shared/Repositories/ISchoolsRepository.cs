using SchoolService.Domain.Schools;
using SchoolService.Domain.Shared.Pagination;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolService.Domain.Shared.Repositories;
public interface ISchoolsRepository
{
    Task<School?> GetSchool(int id);
    Task<PagedResponse<School>> SearchSchoolsByName(string? name, int pageNumber, int pageSize);
    Task<int> CreateSchool(School input);
    Task<bool> UpdateSchool(School input);
    Task<bool> DeleteSchool(School input);
    Task<bool> UpdateClass(ClassRoom input);
}
