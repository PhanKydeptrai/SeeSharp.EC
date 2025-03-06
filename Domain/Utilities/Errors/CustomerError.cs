using Domain.Entities.VerificationTokens;
using SharedKernel;

namespace Domain.Utilities.Errors;

public static class CustomerError
{
    public static Error NotFoundToken(VerificationTokenId verificationTokenId) => Error.NotFound(
        "Customer.NotFoundToken",
        $"The VerificationToken with the Id = '{verificationTokenId}' was not found");

    public static Error TokenExpired(VerificationTokenId verificationTokenId) => Error.Problem(
        "Customer.TokenExpired",
        $"The VerificationToken with the Id = '{verificationTokenId}' has expired");
    
    public static Error LoginFailed() => Error.Problem(
        "Customer.LoginFailed",
        "The login failed. Please check your email and password");

    public static Error RefreshTokenNotFound() => Error.Problem(
        "Customer.RefreshTokenNotFound",
        "The refresh token was not found");

    public static Error RefreshTokenIsBlackList() => Error.Problem(
        "Customer.RefreshTokenIsBlackList",
        "The refresh token is black list");

    public static Error RefreshTokenExpired() => Error.Problem(
        "Customer.RefreshTokenExpired",
        "The refresh token has expired");
}
