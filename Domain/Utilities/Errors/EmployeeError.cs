using SharedKernel;

namespace Domain.Utilities.Errors;

public static class EmployeeError
{
    public static Error LoginFailed() => Error.Problem(
        "Employee.LoginFailed",
        "The login failed. Please check your email and password");
        
    public static Error InValidInformation() => Error.NotFound(
        "Employee.InValidInformation",
        "The information is invalid");
        
    public static Error PasswordNotMatch() => Error.Problem(
        "Employee.PasswordNotMatch",
        "The password does not match");
        
    public static Error NotFoundToken(Guid verificationTokenId) => Error.NotFound(
        "Employee.NotFoundToken",
        $"The VerificationToken with the Id = '{verificationTokenId}' was not found");
        
    public static Error TokenExpired(Guid verificationTokenId) => Error.Problem(
        "Employee.TokenExpired",
        $"The VerificationToken with the Id = '{verificationTokenId}' has expired");
        
    public static Error InValidToken() => Error.Problem(
        "Employee.InValidToken",
        "The token is invalid");
        
    public static Error FailToChangePassword() => Error.Problem(
        "Employee.FailToChangePassword",
        "Failed to change the password");
        
    public static Error RefreshTokenInvalid() => Error.Problem(
        "Employee.RefreshTokenInvalid",
        "The refresh token is invalid");
        
    public static Error NotFound(Guid employeeId) => Error.NotFound(
        "Employee.NotFound",
        $"The employee with the Id = '{employeeId}' was not found");
} 