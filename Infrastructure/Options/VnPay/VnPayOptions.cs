namespace Infrastructure.Options.VnPay;

public sealed class VnPayOptions
{
    public const string ConfigurationSection = "VNPAY";
    public string VNP_URL { get; init; } = string.Empty;
    public string VNP_TMN_CODE { get; init; } = string.Empty;
    public string VNP_RETURNURL_ORDERS { get; init; } = string.Empty;
    public string VNP_HASH_SECRET { get; init; } = string.Empty;
}