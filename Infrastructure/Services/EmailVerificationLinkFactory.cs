using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using SharedKernel.Constants;

namespace Infrastructure.Services;

internal sealed class EmailVerificationLinkFactory
{
    private readonly string _baseUrl;
    private readonly LinkGenerator _linkGenerator;

    public EmailVerificationLinkFactory(IConfiguration configuration, LinkGenerator linkGenerator)
    {
        _baseUrl = configuration["Application:BaseUrl"]
            ?? throw new InvalidOperationException("Base URL is not configured.");
        _linkGenerator = linkGenerator;
    }

    public string CreateLinkForEmailVerification(Guid verificationTokenId)
    {
        string? link = _linkGenerator.GetPathByRouteValues(
            EndpointName.Customer.Verify, 
            new { token = verificationTokenId });

        if (link == null)
        {
            throw new InvalidOperationException("Could not generate verification link.");
        }

        return $"{_baseUrl}{link}";
    }

    public string CreateLinkForChangePassword(Guid verificationTokenId)
    {
        string? link = _linkGenerator.GetPathByRouteValues(
            EndpointName.Customer.ChangePasswordConfirm,
            new { token = verificationTokenId });

        if (link == null)
        {
            throw new InvalidOperationException("Could not generate change password link.");
        }

        return $"{_baseUrl}{link}";
    }
}
