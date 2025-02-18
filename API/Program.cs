using System.Reflection;
using API.Extentions;
using API.Services;
using Application;
using Application.Abstractions.LinkService;
using HealthChecks.UI.Client;
using Infrastructure;
using Infrastructure.MessageBroker;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Persistence;
using Serilog;

var builder = WebApplication.CreateBuilder(args);
//Cấu hình Authen và Author
builder.Services.AddAuthentication();
builder.Services.AddAuthorization();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGenWithAuth(); //* Cấu hình tự viết 
builder.Services.AddHealthChecks();
builder.Services.AddEndpoints(Assembly.GetExecutingAssembly());

#region Dependency Injection
builder.Services.AddApplication(builder.Configuration)
    .AddPersistnce(builder.Configuration)
    .AddInfrastructure(builder.Configuration);

builder.Services.AddScoped<ILinkServices, LinkServices>(); //* Hateoas 
builder.Services.AddHttpContextAccessor();

#endregion
builder.Services.AddCustomProblemDetails();
builder.Host.UseSerilog((context, loggerConfig) =>
loggerConfig.ReadFrom.Configuration(context.Configuration));
builder.Services.Cors();

#region Cấu hình Masstransit

builder.Services.Configure<MessageBrokerSetting>(
    builder.Configuration.GetSection("MessageBroker"));

//builder.Services.AddSingleton(
//    sp => sp.GetRequiredService<IOptions<MessageBrokerSetting>>().Value);
#endregion

var app = builder.Build();
app.UseStatusCodePages();


#region Health Check

app.MapHealthChecks("api/health", new HealthCheckOptions
{
    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse

}); //Kiểm tra tình trạng hoạt động của api 
#endregion

#region Cấu hình Open Telemetry
//TODO: Thêm cấu hình cho OpenTelemetry
//Add open telemetry
// builder.Services.AddOpenTelemetry()
//     .ConfigureResource(re => re.AddService("MyService"))
//     .WithTracing(tracing =>
//     {
//         tracing.AddAspNetCoreInstrumentation()
//             .AddHttpClientInstrumentation()
//             .AddNpgsql();
//     }).UseOtlpExporter();
#endregion

#region Serilog


app.UseSerilogRequestLogging(); //Serilog middleware

app.UseRequestContextLogging(); //Middleware log thông tin request

//Seq
//builder.Services.AddLogging(loggingBuilder =>
//{
//    loggingBuilder.AddSeq();
//});

#endregion

#region Cors
app.UseCors("AllowAll");
#endregion

#region Authen và Author

#endregion

app.UseAuthentication();
app.UseAuthorization();
app.UseSwaggerAndScalar();

app.UseHttpsRedirection();


#region Cấu hình minimal API

app.MapEndpoints(); 
#endregion

app.Run();

