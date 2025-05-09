namespace Infrastructure.Options.GituhubAuth;

public sealed class GithubOptions
{
    public const string ConfigurationSection = "Github";
    public string ClientSecret { get; init; } = string.Empty;
    public string ClientId { get; init; } = string.Empty;
}
