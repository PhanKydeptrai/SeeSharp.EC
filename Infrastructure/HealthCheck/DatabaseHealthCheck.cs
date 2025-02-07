namespace Infrastructure.HealthCheck;

//internal sealed class DatabaseHealthCheck : IHealthCheck
//{
//    private readonly NextSharpReadOnlyDbContext _primarydbContext;

//    public DatabaseHealthCheck(
//        NextSharpReadOnlyDbContext primarydbContext)
//    {
//        _primarydbContext = primarydbContext;
//    }

//    public async Task<HealthCheckResult> CheckHealthAsync(
//        HealthCheckContext context,
//        CancellationToken cancellationToken = default)
//    {
//        try
//        {
//            await _primarydbContext.Database.CanConnectAsync(cancellationToken);
//            return HealthCheckResult.Healthy();
//        }
//        catch
//        {
//            return HealthCheckResult.Unhealthy();
//        }
//    }
//}
