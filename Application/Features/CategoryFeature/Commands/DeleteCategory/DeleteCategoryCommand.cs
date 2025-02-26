using Application.Abstractions.Messaging;

namespace Application.Features.CategoryFeature.Commands.DeleteCategory;

public sealed record DeleteCategoryCommand(Guid categoryId) : ICommand;
