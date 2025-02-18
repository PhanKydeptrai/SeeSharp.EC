using Application.Abstractions.LinkService;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Pages;

public class PagedList<T>(List<T> items, int page, int pageSize, int totalCount)
{
    public List<T> Items { get; set; } = items;
    public int Page { get; set; } = page;
    public int PageSize { get; set; } = pageSize;
    public int TotalCount { get; set; } = totalCount;
    public bool HasNextPage => Page * PageSize < TotalCount;
    public bool HaspreviousPage => Page > 1;
    public List<Link> Links { get; set; } = new();

    public static async Task<PagedList<T>> CreateAsync(IQueryable<T> query, int page, int pageSize)
    {
        var totalCount = await query.CountAsync();
        var items = await query.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();

        return new(items, page, pageSize, totalCount);
    }
}
