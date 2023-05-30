using FluentValidation.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolService.Domain.Shared.Exceptions;
public class DataNotValidException : Exception
{
    public int StatusCode { get; set; } = 422;
    public List<ValidationFailure>? Errors { get; set; }
    public string? HttpResponseMessage { get; set; }
    public DataNotValidException(string? message = "The data not valid", List<ValidationFailure>? errors = null) : base(message)
    {
        HttpResponseMessage = message;
        Errors = errors;
    }
}