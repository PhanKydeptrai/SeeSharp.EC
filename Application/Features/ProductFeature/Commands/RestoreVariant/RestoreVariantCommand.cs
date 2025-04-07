using Application.Abstractions.Messaging;

namespace Application.Features.ProductFeature.Commands.RestoreVariant;

public record RestoreVariantCommand(Guid VariantId) : ICommand;
