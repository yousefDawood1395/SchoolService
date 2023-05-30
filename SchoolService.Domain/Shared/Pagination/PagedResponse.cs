using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolService.Domain.Shared.Pagination;
public class PagedResponse<T>
{
    public int TotalCount { get; set; }
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 10;
    public IList<T> Data { get; set; } = new List<T>();
}
