using System.Reflection;
using System.Security.Claims;
using API.Extentions;
using API.Services;
using Application;
using Application.Abstractions.LinkService;
using Application.Security;
using HealthChecks.UI.Client;
using Infrastructure;
using Infrastructure.MessageBroker;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.IdentityModel.Tokens;
using Persistence;
using Serilog;

var builder = WebApplication.CreateBuilder(args);
//TODO: Move to external file
//Cấu hình Authen và Author 
#region Need to move to external file
builder.Services.AddAuthentication(options =>
{

    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;

})
.AddJwtBearer(options =>
{
    options.Events = new JwtBearerEvents
    {
        OnTokenValidated = async context =>
        {
            var jti = context.Principal?.FindFirst("jti")?.Value;
            if (jti is null)
            {
                var revocationService = context.HttpContext.RequestServices
                    .GetRequiredService<ITokenRevocationService>();
                if (await revocationService.IsTokenRevoked(jti!))
                {
                    context.Fail("Token was revoked.");
                }
            }
        }
    };

    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidIssuer = builder.Configuration["Issuer"],
        ValidateAudience = true,
        ValidAudience = builder.Configuration["Audience"],
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(
        System.Text.Encoding.UTF8.GetBytes(builder.Configuration["SigningKey"]!)),
        ValidateLifetime = true, // Kiểm tra thời gian hết hạn của token
        ClockSkew = TimeSpan.Zero, // Loại bỏ thời gian trễ mặc định
                                   // Đảm bảo token chứa claim về vai trò
        RoleClaimType = ClaimTypes.Role
    };
});
#endregion

builder.Services.AddAuthorization();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGenWithAuth(); //* Cấu hình tự viết
builder.Services.AddHealthChecks();
builder.Services.AddEndpoints(Assembly.GetExecutingAssembly());

#region Dependency Injection
builder.Services.AddApplication()
    .AddPersistnce(builder.Configuration)
    .AddInfrastructure(builder.Configuration);

builder.Services.AddScoped<ILinkServices, LinkServices>(); //* Hateoas 

// builder.Services.AddHttpContextAccessor();

#endregion
builder.Services.AddCustomProblemDetails();
builder.Host.UseSerilog((context, loggerConfig) =>
loggerConfig.ReadFrom.Configuration(context.Configuration));
//Seq
builder.Services.AddLogging(loggingBuilder =>
{
    loggingBuilder.AddSeq();
});
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

