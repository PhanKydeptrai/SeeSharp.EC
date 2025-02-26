using Application.Abstractions.Messaging;
using Microsoft.AspNetCore.Http;

namespace Application.Features.CategoryFeature.Commands.UpdateCategory;

public sealed record UpdateCategoryCommand(
    Guid categoryId,
    string categoryName,
    IFormFile? categoryImage) : ICommand;