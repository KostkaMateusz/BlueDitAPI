using Bluedit.Application.Exceptions;

namespace Bluedit.Middlewares;

public class ErrorHandlingMiddleware() : IMiddleware
{
    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        try
        {
            await next.Invoke(context);
        }
        catch (NotFoundException notFound)
        {
            context.Response.StatusCode = 404;
            await context.Response.WriteAsync(notFound.Message);
        }
        catch (ForbidException)
        {
            context.Response.StatusCode = 403;
            await context.Response.WriteAsync("Access forbidden");
        }
        catch (Exception ex)
        {
            context.Response.StatusCode = 500;
            await context.Response.WriteAsync("Something went wrong");
        }
    }
}