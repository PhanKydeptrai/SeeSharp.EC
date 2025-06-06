﻿using Application.Abstractions.Messaging;
using Microsoft.AspNetCore.Http;

namespace Application.Features.ProductFeature.Commands.CreateProduct;

public record CreateProductCommand(
    string ProductName,
    IFormFile? ProductImage,
    string ProductBaseVariantName,
    string ColorCode,
    string? Description,
    decimal VariantPrice,
    Guid? CategoryId) : ICommand;
