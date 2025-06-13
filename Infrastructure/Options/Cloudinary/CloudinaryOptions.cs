namespace Infrastructure.Options.Cloudinary;

public sealed class CloudinaryOptions
{
    public const string ConfigurationSection = "Cloudinary";
    public string ApiKey { get; init; } = string.Empty;
}
