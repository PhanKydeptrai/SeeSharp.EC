using Microsoft.Extensions.Configuration;
using Supabase;

namespace Application.Services;

public interface ICustomerSupabaseClient
{
    Supabase.Client Client { get; }
}

public interface IAdminSupabaseClient
{
    Supabase.Client Client { get; }
}

public class CustomerSupabaseClient : ICustomerSupabaseClient
{
    public Supabase.Client Client { get; }
    
    public CustomerSupabaseClient(IConfiguration configuration)
    {
        Client = new Supabase.Client(
            configuration["Supabase:Url_Customer"]!,
            configuration["Supabase:Key_Customer"],
            new SupabaseOptions
            {
                AutoRefreshToken = true,
                AutoConnectRealtime = true
            });
    }
}


public class AdminSupabaseClient : IAdminSupabaseClient
{
    public Supabase.Client Client { get; }
    
    public AdminSupabaseClient(IConfiguration configuration)
    {
        Client = new Supabase.Client(
            configuration["Supabase:Url_Admin"]!,
            configuration["Supabase:Key_Admin"],
            new SupabaseOptions
            {
                AutoRefreshToken = true,
                AutoConnectRealtime = true
            });
    }
}