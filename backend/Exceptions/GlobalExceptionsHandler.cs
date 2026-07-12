using Microsoft.AspNetCore.Mvc;

namespace backend.Exceptions;

internal sealed class GlobalExceptionsHandler(RequestDelegate next) 
{

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await next(context);
        }
        catch (Exception ex)
        {
            context.Response.StatusCode = ex switch
            {
                NotFoundException => StatusCodes.Status404NotFound,
                BusinessException or ValidationException => StatusCodes.Status400BadRequest,
                _ => StatusCodes.Status500InternalServerError
            };

            await context.Response.WriteAsJsonAsync(new ProblemDetails
            {
                Type = ex.GetType().Name,
                Status = context.Response.StatusCode,
                Title = "Ocorreu um erro.",
                Detail = ex.Message
            });
        }
    }
}
