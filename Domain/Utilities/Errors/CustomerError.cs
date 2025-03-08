using Domain.Entities.VerificationTokens;
using SharedKernel;

namespace Domain.Utilities.Errors;

public static class CustomerError
{
    public static Error NotFoundToken(VerificationTokenId verificationTokenId) => Error.NotFound(
        "Customer.NotFoundToken",
        $"The VerificationToken with the Id = '{verificationTokenId}' was not found");
    public static Error PasswordNotMatch() => Error.Problem(
        "Customer.PasswordNotMatch",
        "The password does not match");
    public static Error TokenExpired(VerificationTokenId verificationTokenId) => Error.Problem(
        "Customer.TokenExpired",
        $"The VerificationToken with the Id = '{verificationTokenId}' has expired");
    public static Error VerificationTokenInvalid(VerificationTokenId verificationTokenId) => Error.Problem(
        "Customer.TokenExpired",
        $"The VerificationToken with the Id = '{verificationTokenId}' has expired");
    public static Error LoginFailed() => Error.Problem(
        "Customer.LoginFailed",
        "The login failed. Please check your email and password");
    public static Error RefreshTokenInvalid() => Error.Problem(
        "Customer.RefreshTokenInvalid",
        "The refresh token is invalid");
    public static Error FailToChangePassword() => Error.Problem(
        "Customer.FailToChangePassword",
        "Failed to change the password");
}
