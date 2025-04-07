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

    public string CreateLinkForEmailVerification(Guid TokenId)
    {
        string? link = _linkGenerator.GetPathByRouteValues(
            EndpointName.Customer.Verify, 
            new { verificationTokenId = TokenId });

        if (link == null)
        {
            throw new InvalidOperationException("Could not generate verification link.");
        }
        
        return $"{_baseUrl}{link}";
    }

    public string CreateLinkForChangePassword(Guid tokenId)
    {
        string? link = _linkGenerator.GetPathByRouteValues(
            EndpointName.Customer.ChangePasswordConfirm,
            new { verificationTokenId = tokenId });

        if (link == null)
        {
            throw new InvalidOperationException("Could not generate change password link.");
        }

        return $"{_baseUrl}{link}";
    }

    public string CreateLinkForResetPassword(Guid tokenId)
    {
        string? link = _linkGenerator.GetPathByRouteValues(
            EndpointName.Customer.ResetPasswordConfirm,
            new { verificationTokenId = tokenId });

        if (link == null)
        {
            throw new InvalidOperationException("Could not generate reset password link.");
        }

        return $"{_baseUrl}{link}";
    }
    
    public string CreateLinkForEmployeeResetPassword(Guid tokenId)
    {
        string? link = _linkGenerator.GetPathByRouteValues(
            "EmployeeResetPasswordConfirm",
            new { verificationTokenId = tokenId });

        if (link == null)
        {
            throw new InvalidOperationException("Could not generate employee reset password link.");
        }

        return $"{_baseUrl}{link}";
    }
    
    public string CreateLinkForEmployeeChangePassword(Guid tokenId)
    {
        string? link = _linkGenerator.GetPathByRouteValues(
            "EmployeeChangePasswordConfirm",
            new { verificationTokenId = tokenId });

        if (link == null)
        {
            throw new InvalidOperationException("Could not generate employee change password link.");
        }

        return $"{_baseUrl}{link}";
    }
}
