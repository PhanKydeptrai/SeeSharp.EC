using Domain.Database.PostgreSQL.ReadModels;
using System.Collections.Generic;
using Xunit;

namespace SeeSharp.EC.Application.UnitTest.ReadModels;

public class CategoryReadModelTests
{
    [Fact]
    public void CategoryReadModel_Properties_Should_Be_Set_Correctly()
    {
        // Arrange
        var categoryId = Ulid.NewUlid();
        var categoryName = "Test Category";

        // Act
        var categoryReadModel = new CategoryReadModel
        {
            CategoryId = categoryId,
            CategoryName = categoryName,
            ProductReadModels = new List<ProductReadModel>()
        };

        // Assert
        Assert.Equal(categoryId, categoryReadModel.CategoryId);
        Assert.Equal(categoryName, categoryReadModel.CategoryName);
        Assert.NotNull(categoryReadModel.ProductReadModels);
        Assert.Empty(categoryReadModel.ProductReadModels);
    }

    [Fact]
    public void CategoryReadModel_Can_Have_Products()
    {
        // Arrange
        var categoryId = Ulid.NewUlid();
        var productId1 = Ulid.NewUlid();
        var productId2 = Ulid.NewUlid();

        var product1 = new ProductReadModel
        {
            ProductId = productId1,
            ProductName = "Product 1",
            CategoryId = categoryId
        };

        var product2 = new ProductReadModel
        {
            ProductId = productId2,
            ProductName = "Product 2",
            CategoryId = categoryId
        };

        // Act
        var categoryReadModel = new CategoryReadModel
        {
            CategoryId = categoryId,
            CategoryName = "Test Category",
            ProductReadModels = new List<ProductReadModel> { product1, product2 }
        };

        // Assert
        Assert.NotNull(categoryReadModel.ProductReadModels);
        Assert.Equal(2, categoryReadModel.ProductReadModels.Count);
        
        Assert.Contains(categoryReadModel.ProductReadModels, p => p.ProductId == productId1);
        Assert.Contains(categoryReadModel.ProductReadModels, p => p.ProductId == productId2);
        
        var foundProduct1 = categoryReadModel.ProductReadModels.First(p => p.ProductId == productId1);
        Assert.Equal("Product 1", foundProduct1.ProductName);
        
        var foundProduct2 = categoryReadModel.ProductReadModels.First(p => p.ProductId == productId2);
        Assert.Equal("Product 2", foundProduct2.ProductName);
    }
} 