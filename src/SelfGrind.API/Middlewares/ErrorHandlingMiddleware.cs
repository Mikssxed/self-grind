using System.Text.Json;
using SelfGrind.Domain.Exceptions;

namespace SelfGrind.Middlewares;

public class ErrorHandlingMiddleware(ILogger<ErrorHandlingMiddleware> logger) : IMiddleware
{
    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        try
        {
            await next.Invoke(context);
        }
        catch (BadRequestException ex)
        {
            context.Response.StatusCode = StatusCodes.Status400BadRequest;
            context.Response.ContentType = "application/json";

            var errorResponse = new
            {
                message = ex.Message,
                errors = ex.Errors
            };

            await context.Response.WriteAsJsonAsync(errorResponse);
            logger.LogWarning(ex, "Bad request: {Message}", ex.Message);
        }
        catch (NotFoundException ex)
        {
            context.Response.StatusCode = StatusCodes.Status404NotFound;
            context.Response.ContentType = "application/json";

            var errorResponse = new { message = ex.Message };
            await context.Response.WriteAsJsonAsync(errorResponse);

            logger.LogWarning(ex.Message);
        }
        catch (ForbidException ex)
        {
            context.Response.StatusCode = StatusCodes.Status403Forbidden;
            context.Response.ContentType = "application/json";

            var errorResponse = new { message = "You do not have permission to perform this action." };
            await context.Response.WriteAsJsonAsync(errorResponse);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, ex.Message);

            context.Response.StatusCode = StatusCodes.Status500InternalServerError;
            context.Response.ContentType = "application/json";

            var errorResponse = new { message = "Something went wrong." };
            await context.Response.WriteAsJsonAsync(errorResponse);
        }
    }
}