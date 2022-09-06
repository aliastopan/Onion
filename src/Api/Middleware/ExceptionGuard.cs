using System.Net;
using System.Text.Json;
using System.Diagnostics;

namespace Onion.Api.Middleware;

public class ExceptionGuard
{
    private readonly RequestDelegate _next;

    public ExceptionGuard(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch(Exception exception)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            await context.Response.WriteAsync(JsonSerializer
                .Serialize(new
                {
                    title = "Something went wrong.",
                    code = "500",
                    detail = exception.Message,
                    stackTrace = exception.Demystify().StackTrace
                }));
        }
    }
}
