using Application.Abstractions.Messaging;
using Domain.Entities.Categories;
using Microsoft.AspNetCore.Http;

namespace Application.Features.CategoryFeature.Commands.CreateCategory;

public sealed record CreateCategoryCommand(
    string categoryName,
    IFormFile? image) : ICommand<CategoryId>;