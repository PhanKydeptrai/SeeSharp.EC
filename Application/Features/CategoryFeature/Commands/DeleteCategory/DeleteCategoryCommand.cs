using Application.Abstractions.Messaging;

namespace Application.Features.CategoryFeature.Commands.DeleteCategory;

public record DeleteCategoryCommand(string categoryId) : ICommand;
