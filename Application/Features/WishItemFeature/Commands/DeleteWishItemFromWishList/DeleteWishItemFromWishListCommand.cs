using Application.Abstractions.Messaging;

namespace Application.Features.WishItemFeature.Commands.DeleteWishItemFromWishList;

public record DeleteWishItemFromWishListCommand(Guid WishItemId) : ICommand;

