using Application.Abstractions.Messaging;

namespace Application.Features.ProductFeature.Commands.DeleteVariant;

public record DeleteVariantCommand(Guid VariantId) : ICommand;
