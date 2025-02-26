using Application.Abstractions.Messaging;

namespace Application.Features.CategoryFeature.Commands.RestoreCategory;

public record RestoreCategoryCommand(Guid CategoryId) : ICommand;
