using Domain.Database.PostgreSQL.ReadModels;
using Domain.Entities.Products;
using Domain.ReadModels;
using System.Collections.Generic;
using Xunit;

namespace SeeSharp.EC.Application.UnitTest.ReadModels;

public class ProductReadModelTests
{
    [Fact]
    public void ProductReadModel_Properties_Should_Be_Set_Correctly()
    {
        // Arrange
        var productId = Ulid.NewUlid();
        var productName = "Test Product";
        var imageUrl = "http://example.com/product.jpg";
        var description = "Test product description";
        var productStatus = ProductStatus.InStock;
        var categoryId = Ulid.NewUlid();
        var categoryReadModel = new CategoryReadModel
        {
            CategoryId = categoryId,
            CategoryName = "Test Category"
        };

        // Act
        var productReadModel = new ProductReadModel
        {
            ProductId = productId,
            ProductName = productName,
            ImageUrl = imageUrl,
            Description = description,
            ProductStatus = productStatus,
            CategoryId = categoryId,
            CategoryReadModel = categoryReadModel,
            ProductVariantReadModels = new List<ProductVariantReadModel>()
        };

        // Assert
        Assert.Equal(productId, productReadModel.ProductId);
        Assert.Equal(productName, productReadModel.ProductName);
        Assert.Equal(imageUrl, productReadModel.ImageUrl);
        Assert.Equal(description, productReadModel.Description);
        Assert.Equal(productStatus, productReadModel.ProductStatus);
        Assert.Equal(categoryId, productReadModel.CategoryId);
        Assert.NotNull(productReadModel.CategoryReadModel);
        Assert.Equal(categoryId, productReadModel.CategoryReadModel.CategoryId);
        Assert.Equal("Test Category", productReadModel.CategoryReadModel.CategoryName);
        Assert.NotNull(productReadModel.ProductVariantReadModels);
        Assert.Empty(productReadModel.ProductVariantReadModels);
    }

    [Fact]
    public void ProductReadModel_Can_Have_ProductVariants()
    {
        // Arrange
        var productId = Ulid.NewUlid();
        var categoryId = Ulid.NewUlid();
        var variantId1 = Ulid.NewUlid();
        var variantId2 = Ulid.NewUlid();

        var variant1 = new ProductVariantReadModel
        {
            ProductVariantId = variantId1,
            VariantName = "Variant 1",
            ProductVariantPrice = 100.0m
        };

        var variant2 = new ProductVariantReadModel
        {
            ProductVariantId = variantId2,
            VariantName = "Variant 2",
            ProductVariantPrice = 200.0m
        };

        // Act
        var productReadModel = new ProductReadModel
        {
            ProductId = productId,
            ProductName = "Test Product",
            CategoryId = categoryId,
            CategoryReadModel = new CategoryReadModel { CategoryId = categoryId },
            ProductVariantReadModels = new List<ProductVariantReadModel> { variant1, variant2 }
        };

        // Assert
        Assert.NotNull(productReadModel.ProductVariantReadModels);
        Assert.Equal(2, productReadModel.ProductVariantReadModels.Count);
        
        Assert.Contains(productReadModel.ProductVariantReadModels, v => v.ProductVariantId == variantId1);
        Assert.Contains(productReadModel.ProductVariantReadModels, v => v.ProductVariantId == variantId2);
        
        var foundVariant1 = productReadModel.ProductVariantReadModels.First(v => v.ProductVariantId == variantId1);
        Assert.Equal("Variant 1", foundVariant1.VariantName);
        Assert.Equal(100.0m, foundVariant1.ProductVariantPrice);
        
        var foundVariant2 = productReadModel.ProductVariantReadModels.First(v => v.ProductVariantId == variantId2);
        Assert.Equal("Variant 2", foundVariant2.VariantName);
        Assert.Equal(200.0m, foundVariant2.ProductVariantPrice);
    }

    [Fact]
    public void ProductReadModel_Optional_Properties_Can_Be_Null()
    {
        // Arrange & Act
        var productReadModel = new ProductReadModel
        {
            ProductId = Ulid.NewUlid(),
            ProductName = "Test Product",
            ProductStatus = ProductStatus.InStock,
            CategoryId = Ulid.NewUlid(),
            CategoryReadModel = new CategoryReadModel(),
            // Optional properties not set
            ImageUrl = null,
            Description = null
        };

        // Assert
        Assert.Null(productReadModel.ImageUrl);
        Assert.Null(productReadModel.Description);
        Assert.NotNull(productReadModel.ProductVariantReadModels);
        Assert.Empty(productReadModel.ProductVariantReadModels);
    }
} 