﻿using Application.DTOs.Category;
using Application.Features.Pages;
using Application.IServices;
using Domain.Entities.Categories;
using Microsoft.Extensions.Caching.Distributed;

namespace Infrastructure.Services.CategoryServices;

internal class CategoryQueryServicesDecorated : ICategoryQueryServices
{
    private readonly ICategoryQueryServices _decorated;
    private readonly IDistributedCache _cache;
    public CategoryQueryServicesDecorated(
        ICategoryQueryServices decorated,
        IDistributedCache cache)
    {
        _decorated = decorated;
        _cache = cache;
    }

    public async Task<CategoryResponse?> GetById(
        CategoryId categoryId, 
        CancellationToken cancellationToken = default)
    {
        var category = await _decorated.GetById(categoryId, cancellationToken);
        return category;
        // string cacheKey = $"CategoryResponse:{categoryId.Value}";
        // string? cachedCategory = await _cache.GetStringAsync(cacheKey, cancellationToken);
        // CategoryResponse? category;
        // if (string.IsNullOrEmpty(cachedCategory))
        // {
        //     category = await _decorated.GetById(categoryId, cancellationToken);

        //     if (category is null)
        //     {
        //         return category;
        //     }

        //     await _cache.SetStringAsync(
        //         cacheKey,
        //         JsonConvert.SerializeObject(category),
        //         cancellationToken);

        //     return category;
        // }
        // return JsonConvert.DeserializeObject<CategoryResponse>(cachedCategory);
    }

    public async Task<CategoryResponse?> GetCategoryDetail(
        CategoryId categoryId, 
        CancellationToken cancellationToken = default)
    {
        return await _decorated.GetCategoryDetail(categoryId, cancellationToken);
    }

    public async Task<List<CategoryInfo>> GetCategoryInfo()
    {
        return await _decorated.GetCategoryInfo();
    }

    public async Task<bool> IsCategoryNameExist(
        CategoryId? categoryId, 
        CategoryName categoryName, 
        CancellationToken cancellationToken = default)
    {
        return await _decorated.IsCategoryNameExist(categoryId ?? null, categoryName, cancellationToken);
    }

    public async Task<bool> IsCategoryStatusNotDeleted(
        CategoryId categoryId, 
        CancellationToken cancellationToken = default)
    {
        return await _decorated.IsCategoryStatusNotDeleted(categoryId, cancellationToken);
    }

    public async Task<PagedList<CategoryResponse>> PagedList(
        string? filter,
        string? searchTerm,
        string? sortColumn,
        string? sortOrder,
        int? page,
        int? pageSize)
    {
        return await _decorated.PagedList(filter, searchTerm, sortColumn, sortOrder, page, pageSize);
    }
}
