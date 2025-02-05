using System.Reflection;
using API.Extentions;
using Application;
using HealthChecks.UI.Client;
using Infrastructure;
using Infrastructure.MessageBroker;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Persistence;
using Serilog;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

builder.Services.AddEndpointsApiExplorer();
//Cấu hình Authen và Author


#region Cấu hình API Document
builder.Services.AddSwaggerGenWithAuth(); //* Cấu hình tự viết 
app.UseSwaggerAndScalar();
#endregion



#region Problem Details
builder.Services.AddCustomProblemDetails();
app.UseStatusCodePages(); 
#endregion



#region Health Check
builder.Services.AddHealthChecks();

app.MapHealthChecks("api/health", new HealthCheckOptions
{
    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse

}); //Kiểm tra tình trạng hoạt động của api 
#endregion


#region Dependency Injection
builder.Services.AddApplication(builder.Configuration)
    .AddPersistnce(builder.Configuration)
    .AddInfrastructure(builder.Configuration);
#endregion

#region Cấu hình Masstransit
builder.Services.Configure<MessageBrokerSetting>(
    builder.Configuration.GetSection("MessageBroker"));

//builder.Services.AddSingleton(
//    sp => sp.GetRequiredService<IOptions<MessageBrokerSetting>>().Value);
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
builder.Host.UseSerilog((context, loggerConfig) =>
loggerConfig.ReadFrom.Configuration(context.Configuration));

app.UseSerilogRequestLogging(); //Serilog middleware

app.UseRequestContextLogging(); //Middleware log thông tin request

//Seq
//builder.Services.AddLogging(loggingBuilder =>
//{
//    loggingBuilder.AddSeq();
//});

#endregion

#region Cors
builder.Services.Cors();
app.UseCors("AllowAll");
#endregion

#region Authen và Author
builder.Services.AddAuthentication();
app.UseAuthentication();
builder.Services.AddAuthorization();
app.UseAuthorization();
#endregion



app.UseHttpsRedirection();


#region Cấu hình minimal API
builder.Services.AddEndpoints(Assembly.GetExecutingAssembly());
app.MapEndpoints(); 
#endregion

app.Run();

