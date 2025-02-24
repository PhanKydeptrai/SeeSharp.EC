using Application.Abstractions.Messaging;

namespace Application.Features.ProductFeature.Commands.UpdateProduct;

public record UpdateProductCommand(Guid productId) : ICommand;
