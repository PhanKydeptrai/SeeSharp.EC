namespace Infrastructure.Options.ApiKey;

public sealed class ApiKeyOptions
{
    public const string ConfigurationSection = "ApiKey";
    public string ApiKey { get; init; } = string.Empty;
}
