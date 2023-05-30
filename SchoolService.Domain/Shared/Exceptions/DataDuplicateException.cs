using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace SchoolService.Domain.Shared.Exceptions;
public class DataDuplicateException : Exception
{
    public int StatusCode { get; set; } = 409;
    public string? HttpResponseMessage { get; set; }
    public DataDuplicateException(string message = "The data was duplicated") : base(message)
    {
        HttpResponseMessage = message;
    }
}