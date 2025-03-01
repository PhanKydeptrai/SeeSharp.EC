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
    

}
