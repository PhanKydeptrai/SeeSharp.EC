using Application.DTOs.Category;
using Application.Features.Pages;
using Application.IServices;
using Domain.Database.PostgreSQL.ReadModels;
using Domain.Entities.Categories;
using Microsoft.EntityFrameworkCore;
using Persistence.Database.MySQL;
using Persistence.Database.PostgreSQL;
using System.Linq.Expressions;

namespace Infrastructure.Services.CategoryServices;

internal class CategoryQueryServices : ICategoryQueryServices
{
    #region Dependency
    private readonly NextSharpPostgreSQLReadDbContext _contextPostgreSQL;
    public CategoryQueryServices(
        NextSharpPostgreSQLReadDbContext contextPostgreSQL)
    {
        _contextPostgreSQL = contextPostgreSQL;
    }
    #endregion
    public async Task<CategoryResponse?> GetById(
        CategoryId categoryId,
        CancellationToken cancellationToken)
    {
        var categoryResponse = await _contextPostgreSQL.Categories
            .Where(a => a.CategoryId.ToGuid() == categoryId
            && a.CategoryStatus != CategoryStatus.Deleted.ToString())
            .Select(a => new CategoryResponse(
                a.CategoryId.ToGuid(),
                a.CategoryName,
                a.ImageUrl,
                a.CategoryStatus,
                a.IsDefault))
            .FirstOrDefaultAsync();

        return categoryResponse;
    }

    public async Task<bool> IsCategoryNameExist(
        CategoryId? categoryId, 
        CategoryName categoryName, 
        CancellationToken cancellationToken = default)
    {
        if(categoryId is not null)
        {
            return await _contextPostgreSQL.Categories
                .AnyAsync(
                    a => a.CategoryName == categoryName.Value 
                    && a.CategoryId != new Ulid(categoryId.Value));
        }

        return await _contextPostgreSQL.Categories
            .AnyAsync(a => a.CategoryName == categoryName.Value);

    }

    public async Task<PagedList<CategoryResponse>> PagedList(
        string? filter,
        string? searchTerm,
        string? sortColumn,
        string? sortOrder,
        int? page,
        int? pageSize)
    {
        var categoriesQuery = _contextPostgreSQL.Categories.AsQueryable();
        //Search
        if (!string.IsNullOrEmpty(searchTerm))
        {
            categoriesQuery = categoriesQuery.Where(
                x => x.CategoryName.Contains(searchTerm));
        }

        //Filter
        if (!string.IsNullOrEmpty(filter))
        {
            categoriesQuery = categoriesQuery.Where(x => x.CategoryStatus == filter);
        }

        //sort
        Expression<Func<CategoryReadModel, object>> keySelector = sortColumn?.ToLower() switch
        {
            "categoryname" => x => x.CategoryName,
            "categoryid" => x => x.CategoryId,
            _ => x => x.CategoryId
        };

        if (sortOrder?.ToLower() == "desc")
        {
            categoriesQuery = categoriesQuery.OrderByDescending(keySelector);
        }
        else
        {
            categoriesQuery = categoriesQuery.OrderBy(keySelector);
        }

        //paged
        var categories = categoriesQuery
            .Select(a => new CategoryResponse(
                a.CategoryId.ToGuid(),
                a.CategoryName,
                a.ImageUrl,
                a.CategoryStatus,
                a.IsDefault)).AsQueryable();
        var categoriesList = await PagedList<CategoryResponse>
            .CreateAsync(categories, page ?? 1, pageSize ?? 10);

        return categoriesList;
    }
}
