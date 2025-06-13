namespace Infrastructure.Options.Google;

public class GoogleOptions
{
    public const string ConfigurationSection = "Google";
    public string ClientSecret { get; init; } = string.Empty;
    public string ClientId { get; init; } = string.Empty;
}
