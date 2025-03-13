using Microsoft.AspNetCore.Mvc;

namespace API.Middleware;

public class ExceptionHandlingMiddleware(ILogger<ExceptionHandlingMiddleware> logger, RequestDelegate next)
{
    // public async Task InvokeAsync(HttpContext context)
    // {
    //     try
    //     {
    //         await next(context);
    //     }
    //     catch (Exception ex)
    //     {
    //         logger.LogError(ex, "Exception occured: {Message}", ex.Message);
    //         var problemDetail = new ProblemDetails
    //         {
    //             Status = StatusCodes.Status500InternalServerError,
    //             Title = "An error occurred while processing your request"
    //         };
    //         context.Response.StatusCode = StatusCodes.Status500InternalServerError;
    //         await context.Response.WriteAsJsonAsync(problemDetail);
    //     }
    // }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await next(context);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Exception occurred: {Message}", ex.Message);
            await HandleExceptionAsync(context, ex);
        }
    }

    private Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        context.Response.ContentType = "application/json";

        if (exception is BadHttpRequestException badHttpRequestException)
        {
            // Xử lý lỗi BadHttpRequestException
            context.Response.StatusCode = StatusCodes.Status415UnsupportedMediaType;
            var problemDetail = new ProblemDetails
            {
                Status = StatusCodes.Status400BadRequest,
                Title = "☝🤓",
                Detail = badHttpRequestException.Message // Ví dụ: "Failed to bind parameter \"decimal Price\" from \"a\"."
            };
            return context.Response.WriteAsJsonAsync(problemDetail);
        }
        else
        {
            // Xử lý các ngoại lệ khác với mã 500
            context.Response.StatusCode = StatusCodes.Status500InternalServerError;
            var problemDetail = new ProblemDetails
            {
                Status = StatusCodes.Status500InternalServerError,
                Title = "An error occurred while processing your request",
                Detail = "An unexpected error occurred. Please contact support."
            };
            return context.Response.WriteAsJsonAsync(problemDetail);
        }
    }
}
