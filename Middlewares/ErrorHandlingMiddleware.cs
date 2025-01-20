namespace project_core.Middlewares;

public class ErrorHandlingMiddleware
{
    private readonly RequestDelegate _next;

    public ErrorHandlingMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (IndexOutOfRangeException ex)
        {            
            await HandleExceptionAsync(context, 404, ex.Message);
        }
       
        catch (Exception ex)
        {
            await HandleExceptionAsync(context, 500, "An unexpected error occurred.");
        }
    }

    private static Task HandleExceptionAsync(HttpContext context, int statusCode, string message)
    {
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = statusCode;
        return context.Response.WriteAsync($"{{\"error\": \"{message}\"}}");
    }
}
public static class ErrorHandlingMiddlewareExtensions
{
    public static IApplicationBuilder UseErrorHandlingMiddleware(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<ErrorHandlingMiddleware>();
    }
}