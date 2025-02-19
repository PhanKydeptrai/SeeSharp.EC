using Application.Abstractions.Messaging;
using Microsoft.AspNetCore.Http;

namespace Application.Features.CategoryFeature.Commands.UpdateCategory;

public record UpdateCategoryCommand(
    string categoryId,
    string categoryName,
    IFormFile? categoryImage) : ICommand;