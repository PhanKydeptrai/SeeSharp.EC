using System.Linq.Expressions;
using Application.DTOs.WishItems;
using Application.Features.Pages;
using Application.IServices;
using Domain.Database.PostgreSQL.ReadModels;
using Domain.Entities.Customers;
using Domain.Entities.Products;
using Persistence.Database.PostgreSQL;

namespace Infrastructure.Services.WishItemServices;


internal sealed class WishItemQueryServices : IWishItemQueryServices
{
    private readonly SeeSharpPostgreSQLReadDbContext _dbContext;

    public WishItemQueryServices(SeeSharpPostgreSQLReadDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<PagedList<WishlistResponse>> GetWishList(
        string? productStatusFilter, 
        string? searchTerm, 
        string? sortColumn, 
        string? sortOrder,
        int? page,
        int? pageSize, 
        CustomerId customerId)
    {
        var wishItemQuery = _dbContext.WishItems.AsQueryable();

        //Search
        if (!string.IsNullOrEmpty(searchTerm))
        {
            wishItemQuery = wishItemQuery.Where(
                x => x.ProductReadModel.ProductName.Contains(searchTerm));
        }

        //Filter
        if (!string.IsNullOrWhiteSpace(productStatusFilter))
        {
            wishItemQuery = wishItemQuery.Where(x => x.ProductReadModel.ProductStatus == (ProductStatus)Enum.Parse(typeof(ProductStatus), productStatusFilter));
        }

        //sort
        Expression<Func<WishItemReadModel, object>> keySelector = sortColumn?.ToLower() switch
        {
            "wishitemid" => x => x.WishItemId,
            "productid" => x => x.ProductId,
            "productname" => x => x.ProductReadModel.ProductName,
            "productprice" => x => x.ProductReadModel.ProductPrice,
            _ => x => x.WishItemId
        };

        if (sortOrder?.ToLower() == "desc")
        {
            wishItemQuery = wishItemQuery.OrderByDescending(keySelector);
        }
        else
        {
            wishItemQuery = wishItemQuery.OrderBy(keySelector);
        }

        //paged
        var wishList = wishItemQuery
            .Select(x => new WishlistResponse(
                x.WishItemId.ToGuid(),
                x.ProductId.ToGuid(),
                x.ProductReadModel.ProductName,
                x.ProductReadModel.ImageUrl,
                x.ProductReadModel.Description,
                x.ProductReadModel.ProductPrice,
                x.ProductReadModel.ProductStatus.ToString(),
                x.ProductReadModel.CategoryReadModel.CategoryName)).AsQueryable();
        var wishItemList = await PagedList<WishlistResponse>
            .CreateAsync(wishList, page ?? 1, pageSize ?? 10);

        return wishItemList;
    }
}
