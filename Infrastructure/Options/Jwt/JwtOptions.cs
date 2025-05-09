namespace Infrastructure.Options.Jwt;

public sealed class JwtOptions
{
    public const string ConfigurationSection = "Jwt";
    public string SecSigningKeyret { get; init; } = string.Empty;
    public string Issuer { get; init; } = string.Empty;
    public string Audience { get; init; } = string.Empty;
}
