using API.Middleware;

namespace API.Extentions;

public static class MiddlewareExtensions
{
    public static IApplicationBuilder UseRequestContextLogging(this IApplicationBuilder app)
    {
        //Đăng ký middleware để log thông tin request
        app.UseMiddleware<RequestContextLoggingMiddleware>();
        //Đăng ký middleware xử lý exception
        app.UseMiddleware<ExceptionHandlingMiddleware>();
        return app;
    }
}
