using FluentValidation.Results;
using SchoolService.Domain.Shared.Exceptions;
using System.Diagnostics.CodeAnalysis;

namespace SchoolService.Presentation.ExceptionHandlers;
[ExcludeFromCodeCoverage]
public class HttpException
{
    public string? Message { get; set; }
    public int StatusCode { get; set; }
    public string? StackTrace { get; set; }
    public List<ValidationFailure>? Validations { get; set; }

    public static HttpException FromException(Exception exception)
    {

        return exception switch
        {
            DataNotFoundException ex =>
                new HttpException()
                {
                    Message = ex.HttpResponseMessage,
                    StackTrace = ex.StackTrace,
                    StatusCode = ex.StatusCode
                },
            DataDuplicateException ex =>
                new HttpException()
                {
                    Message = ex.HttpResponseMessage,
                    StackTrace = ex.StackTrace,
                    StatusCode = ex.StatusCode
                },
            DataNotValidException ex =>
                new HttpException()
                {
                    Message = ex.HttpResponseMessage,
                    StackTrace = ex.StackTrace,
                    StatusCode = ex.StatusCode,
                    Validations = ex.Errors
                },
            BusinessException ex =>
                new HttpException()
                {
                    Message = ex.HttpResponseMessage,
                    StackTrace = ex.StackTrace,
                    StatusCode = ex.StatusCode
                },
            _ => new HttpException()
            {
                Message = exception.Message,
                StackTrace = exception.StackTrace,
                StatusCode = 500
            }
        };

    }
}
