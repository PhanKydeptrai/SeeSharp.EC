using SharedKernel;

namespace Domain.Utilities.Errors;

public static class FeedBackError
{
    public static Error NotFound(Guid feedbackId) => Error.NotFound(
        "FeedBack.NotFound",
        $"The feedback with the Id = '{feedbackId}' was not found");

    //public static Error AlreadyExists(Guid feedbackId) => Error.Conflict(
    //    "FeedBack.AlreadyExists",
    //    $"The feedback with the Id = '{feedbackId}' already exists");
    //public static Error InvalidContent() => Error.Problem(
    //    "FeedBack.InvalidContent",
    //    "The feedback content is invalid or empty");
}
