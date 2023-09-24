namespace SampleApp.ErrorHandling;

public class ErrorHandlingMiddleware
{
    private RequestDelegate _next;
    public ErrorHandlingMiddleware(RequestDelegate next)
    {
        _next = next;
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

            context.Response.Redirect("/Error/UnexpectedError");
        }
    }
}
