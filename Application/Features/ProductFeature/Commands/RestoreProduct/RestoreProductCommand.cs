using Application.Abstractions.Messaging;

namespace Application.Features.ProductFeature.Commands.RestoreProduct;

public record RestoreProductCommand(Guid ProductId) : ICommand;
