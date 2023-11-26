namespace SampleApp.ErrorHandling;

public class ErrorHandlingMiddleware
{
    private RequestDelegate _next;
    private readonly ILogger<ErrorHandlingMiddleware> _logger;
    public ErrorHandlingMiddleware(RequestDelegate next, ILogger<ErrorHandlingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task Invoke(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            //1. Logging
            _logger.LogError(ex.Message, ex.StackTrace);

            context.Response.Redirect("/Error/UnexpectedError");
        }
    }
}
