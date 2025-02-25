using Application.Abstractions.Messaging;

namespace Application.Features.ProductFeature.Commands.DeleteProduct;

public record DeleteProductCommand(Guid ProductId) : ICommand;
