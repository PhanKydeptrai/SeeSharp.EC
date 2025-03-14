using Application.Abstractions.Messaging;

namespace Application.Features.WishItemFeature.Commands;

public record AddWishListCommand(Guid ProductId, Guid CustomerId) : ICommand;