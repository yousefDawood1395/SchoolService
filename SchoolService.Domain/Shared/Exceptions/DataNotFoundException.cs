using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolService.Domain.Shared.Exceptions
{
    public class DataNotFoundException : Exception
    {
        public int StatusCode { get; set; } = 404;
        public string? HttpResponseMessage { get; set; }
        public DataNotFoundException(string message = "The data was not found") : base(message)
        {
            HttpResponseMessage = message;
        }
    }
}
