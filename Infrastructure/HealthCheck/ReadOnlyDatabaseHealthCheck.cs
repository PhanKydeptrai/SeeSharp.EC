namespace Infrastructure.HealthCheck;

// internal sealed class ReadOnlyDatabaseHealthCheck : IHealthCheck
// {
//     private readonly NextSharpReadOnlyDbContext _primaryReadOnlyDbContext;

//     public ReadOnlyDatabaseHealthCheck(NextSharpReadOnlyDbContext primaryReadOnlyDbContext)
//     {
//         _primaryReadOnlyDbContext = primaryReadOnlyDbContext;
//     }

//     public async Task<HealthCheckResult> CheckHealthAsync(
//         HealthCheckContext context, 
//         CancellationToken cancellationToken = default)
//     {
//         try
//         {
//             await _primaryReadOnlyDbContext.Database.CanConnectAsync(cancellationToken);
//             return HealthCheckResult.Healthy();
//         }
//         catch
//         {
//             return HealthCheckResult.Unhealthy();
//         }
//     }
// }
