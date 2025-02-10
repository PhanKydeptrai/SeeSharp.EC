using Application.Abstractions.Messaging;
using Microsoft.AspNetCore.Http;

namespace Application.Features.CategoryFeature;

public record CreateCategoryCommand(
    string categoryName,
    IFormFile? image) : ICommand;