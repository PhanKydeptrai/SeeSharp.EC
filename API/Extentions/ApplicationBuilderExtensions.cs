using Scalar.AspNetCore;

namespace API.Extentions;

public static class ApplicationBuilderExtensions
{
    public static IApplicationBuilder UseSwaggerAndScalar(this WebApplication app)
    {

        app.UseSwagger(options =>
        {
            // Cấu hình đường dẫn cho Scalar
            options.RouteTemplate = "/openapi/{documentName}.json";
        });

        // Cấu hình Scalar API Reference
        app.MapScalarApiReference();


        // Cấu hình Swagger UI cho Swagger DOC
        app.UseSwaggerUI(options =>
        {
            options.SwaggerEndpoint("/openapi/v1.json", "NextSharp");
            options.RoutePrefix = "swagger";
        });


        return app;
    }
}
