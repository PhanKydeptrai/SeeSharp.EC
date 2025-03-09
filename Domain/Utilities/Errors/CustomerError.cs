using Domain.Entities.VerificationTokens;
using SharedKernel;

namespace Domain.Utilities.Errors;

public static class CustomerError
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="verificationTokenId"></param>
    /// <returns></returns>
    public static Error NotFoundToken(VerificationTokenId verificationTokenId) => Error.NotFound(
        "Customer.NotFoundToken",
        $"The VerificationToken with the Id = '{verificationTokenId}' was not found");
    
    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public static Error PasswordNotMatch() => Error.Problem(
        "Customer.PasswordNotMatch",
        "The password does not match");
    
    /// <summary>
    /// 
    /// </summary>
    /// <param name="verificationTokenId"></param>
    /// <returns></returns>
    public static Error TokenExpired(VerificationTokenId verificationTokenId) => Error.Problem(
        "Customer.TokenExpired",
        $"The VerificationToken with the Id = '{verificationTokenId}' has expired");
    
    /// <summary>
    /// 
    /// </summary>
    /// <param name="verificationTokenId"></param>
    /// <returns></returns>
    public static Error VerificationTokenInvalid(VerificationTokenId verificationTokenId) => Error.Problem(
        "Customer.TokenExpired",
        $"The VerificationToken with the Id = '{verificationTokenId}' has expired");
    
    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public static Error LoginFailed() => Error.Problem(
        "Customer.LoginFailed",
        "The login failed. Please check your email and password");
    
    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public static Error RefreshTokenInvalid() => Error.Problem(
        "Customer.RefreshTokenInvalid",
        "The refresh token is invalid");
    
    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public static Error FailToChangePassword() => Error.Problem(
        "Customer.FailToChangePassword",
        "Failed to change the password");

    /// <summary>
    /// Google authentication token is invalid
    /// </summary>
    /// <returns></returns>
    public static Error InValidToken() => Error.Problem(
        "Customer.InValidToken",
        "The token is invalid");
}
