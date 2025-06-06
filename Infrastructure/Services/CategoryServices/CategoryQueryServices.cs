﻿using Application.DTOs.Category;
using Application.Features.Pages;
using Application.IServices;
using Domain.Database.PostgreSQL.ReadModels;
using Domain.Entities.Categories;
using Microsoft.EntityFrameworkCore;
using Persistence.Database.PostgreSQL;
using System.Linq.Expressions;

namespace Infrastructure.Services.CategoryServices;

internal class CategoryQueryServices : ICategoryQueryServices
{
    #region Dependency
    private readonly SeeSharpPostgreSQLReadDbContext _contextPostgreSQL;
    public CategoryQueryServices(
        SeeSharpPostgreSQLReadDbContext contextPostgreSQL)
    {
        _contextPostgreSQL = contextPostgreSQL;
    }
    #endregion
    public async Task<CategoryResponse?> GetById(
        CategoryId categoryId,
        CancellationToken cancellationToken)
    {
        var categoryResponse = await _contextPostgreSQL.Categories
            .Where(a => a.CategoryId == new Ulid(categoryId))
            .Select(a => new CategoryResponse(
                a.CategoryId.ToGuid(),
                a.CategoryName,
                a.ImageUrl,
                a.CategoryStatus.ToString(),
                a.IsDefault))
            .FirstOrDefaultAsync();

        return categoryResponse;
    }

    public async Task<CategoryResponse?> GetCategoryDetail(CategoryId categoryId, CancellationToken cancellationToken = default)
    {
        var categoryResponse = await _contextPostgreSQL.Categories
            .Where(a => a.CategoryId == new Ulid(categoryId)
            && a.CategoryStatus != CategoryStatus.Deleted)
            .Select(a => new CategoryResponse(
                a.CategoryId.ToGuid(),
                a.CategoryName,
                a.ImageUrl,
                a.CategoryStatus.ToString(),
                a.IsDefault))
            .FirstOrDefaultAsync();

        return categoryResponse;
    }

    public async Task<bool> IsCategoryNameExist(
        CategoryId? categoryId,
        CategoryName categoryName,
        CancellationToken cancellationToken = default)
    {
        if (categoryId is not null)
        {
            return await _contextPostgreSQL.Categories
                .AnyAsync(
                    a => a.CategoryName == categoryName.Value
                    && a.CategoryId != new Ulid(categoryId.Value));
        }

        return await _contextPostgreSQL.Categories
            .AnyAsync(a => a.CategoryName == categoryName.Value);

    }

    public async Task<bool> IsCategoryStatusNotDeleted(CategoryId categoryId, CancellationToken cancellationToken = default)
    {
        return await _contextPostgreSQL.Categories
            .AnyAsync(a => a.CategoryId == new Ulid(categoryId)
            && a.CategoryStatus != CategoryStatus.Deleted);
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
            categoriesQuery = categoriesQuery.Where(x => x.CategoryStatus == (CategoryStatus)Enum.Parse(typeof(CategoryStatus), filter));
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
                a.CategoryStatus.ToString(),
                a.IsDefault)).AsQueryable();
        var categoriesList = await PagedList<CategoryResponse>
            .CreateAsync(categories, page ?? 1, pageSize ?? 10);

        return categoriesList;
    }

    public async Task<List<CategoryInfo>> GetCategoryInfo()
    {
        return await _contextPostgreSQL.Categories
            .Select(a => new CategoryInfo(a.CategoryId.ToGuid(), a.CategoryName))
            .ToListAsync();
    }
}
