
using System.Linq.Expressions;
using Application.DTOs.Category;
using Application.Features.Pages;
using Application.IServices;
using Domain.Database.PostgreSQL.ReadModels;
using Domain.Entities.Categories;
using Microsoft.EntityFrameworkCore;
using Persistence.Database.PostgreSQL;

namespace Infrastructure.Services.CategoryServices;

internal class CategoryQueryServices : ICategoryQueryServices
{
    private readonly NextSharpPostgreSQLReadDbContext _context;

    public CategoryQueryServices(NextSharpPostgreSQLReadDbContext context)
    {
        _context = context;
    }

    public async Task<CategoryResponse?> GetById(
        CategoryId categoryId,
        CancellationToken cancellationToken)
    {
        return await _context.Categories.Where(a => a.CategoryId == categoryId.Value)
                    .Select(a => new CategoryResponse(
                        a.CategoryId,
                        a.CategoryName,
                        a.ImageUrl,
                        a.CategoryStatus))
                    .FirstOrDefaultAsync();
    }

    public async Task<PagedList<CategoryResponse>> PagedList(
        string? filter,
        string? searchTerm,
        string? sortColumn,
        string? sortOrder,
        int? page = 1,
        int? pageSize = 10)
    {
        var categoriesQuery = _context.Categories.AsQueryable();
        //Search
        if (!string.IsNullOrEmpty(searchTerm))
        {
            categoriesQuery = categoriesQuery.Where(x => x.CategoryName.Contains(searchTerm));
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
                a.CategoryId,
                a.CategoryName,
                a.ImageUrl,
                a.CategoryStatus)).AsQueryable();
        var categoriesList = await PagedList<CategoryResponse>.CreateAsync(categories, page ?? 1, pageSize ?? 10);

        return categoriesList;
    }
}
