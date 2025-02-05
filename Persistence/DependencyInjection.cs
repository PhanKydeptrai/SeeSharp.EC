using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Persistence;

public static class DependencyInjection
{
    public static IServiceCollection AddPersistnce(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddPrimaryDatabase(configuration)
            .AddReadOnlyDatabase(configuration)
            .AddAbstractsReplicaDatabase(configuration);
        return services;
    }

    private static IServiceCollection AddPrimaryDatabase(
        this IServiceCollection services,
        IConfiguration configuration)
    {

        //string connectionString = configuration.GetConnectionString("PrimaryDatabase")
        //    ?? throw new Exception("MySQL connection string is null");
        //services.AddDbContext<NextSharpDbContext>(options =>
        //{
        //    options.UseMySQL(connectionString);
        //});


        return services;
    }

    private static IServiceCollection AddReadOnlyDatabase(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        //string connectionString = configuration.GetConnectionString("ReadOnlyDatabase")
        //    ?? throw new Exception("PostgreSQL connection string is null");
        //services.AddDbContext<NextSharpReadOnlyDbContext>(options =>
        //{
        //    options.UseNpgsql(connectionString);
        //});

        return services;
    }

    private static IServiceCollection AddAbstractsReplicaDatabase(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        //services.AddScoped<INextSharpReadOnlyDbContext>(provider => provider.GetRequiredService<NextSharpReadOnlyDbContext>());
        //services.AddScoped<INextSharpDbContext>(provider => provider.GetRequiredService<NextSharpDbContext>());
        return services;
    }


}
