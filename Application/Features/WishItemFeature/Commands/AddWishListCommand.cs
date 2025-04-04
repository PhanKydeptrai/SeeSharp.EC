using Application.Abstractions.Messaging;

namespace Application.Features.WishItemFeature.Commands;

public record AddWishListCommand(Guid ProductVariantId, Guid CustomerId) : ICommand;