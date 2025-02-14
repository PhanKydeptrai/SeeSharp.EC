using Application.Abstractions.Messaging;
using Microsoft.AspNetCore.Http;

namespace Application.Features.CategoryFeature.Commands.CreateCategory;

public sealed record CreateCategoryCommand(
    string categoryName,
    IFormFile? image) : ICommand;