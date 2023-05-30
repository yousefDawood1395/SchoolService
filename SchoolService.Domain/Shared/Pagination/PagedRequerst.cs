using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolService.Domain.Shared.Pagination;
public class PagedRequerst
{
    public int PageNo { get; set; } = 1;
    public int PageSize { get; set; } = 10;
}
