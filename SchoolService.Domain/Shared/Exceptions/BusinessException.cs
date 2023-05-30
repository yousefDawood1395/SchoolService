using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolService.Domain.Shared.Exceptions
{
    public class BusinessException : Exception
    {
        public int StatusCode { get; set; }
        public string? HttpResponseMessage { get; set; }
        public BusinessException(string message = "There was an error processing your request", int statusCode = 405) : base(message)
        {
            StatusCode = statusCode;
            HttpResponseMessage = message;
        }
    }
}
