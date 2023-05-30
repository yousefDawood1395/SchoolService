namespace SchoolService.Presentation.ExceptionHandlers;
public class ExceptionHandlerMiddleware : IMiddleware
{
    private readonly ILogger<ExceptionHandlerMiddleware> _logger;

    public ExceptionHandlerMiddleware(ILogger<ExceptionHandlerMiddleware> logger)
    {
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        try
        {
            await next(context);
        }
        catch (Exception ex)
        {
            _logger.LogCritical(ex, ex.Message);

            var exception = HttpException.FromException(ex);
            var response = context.Response;
            response.Clear();
            response.StatusCode = exception.StatusCode;
            await response.WriteAsJsonAsync(exception);
            await response.Body.FlushAsync();
        }
    }
}