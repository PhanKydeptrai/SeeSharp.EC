namespace Domain.Utilities.Events.CustomerEvents;

public record SendVerificationEmailToCustomer(string Email, Guid verificationTokenId, Guid MesssageId);