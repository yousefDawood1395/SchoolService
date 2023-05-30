using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolService.Domain.Shared.Exceptions;
public static class GeideaHttpStatusCodes
{
    public const int DataNotFound = 404;
    public const int BusinessException = 405;
    public const int DataNotValid = 422;
    public const int DataDuplicated = 409;
}
