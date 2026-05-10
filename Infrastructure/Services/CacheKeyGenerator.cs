using System.Security.Cryptography;
using System.Text;
using Infrastructure.Helper;

namespace Infrastructure.Services;

internal class CacheKeyGenerator : ICacheKeyGenerator
{
    public string CreateCacheKey(
        string listPrefix,
        string? filterProductStatus,
        string? filterCategory,
        string? sortColumn,
        string? sortOrder,
        int? page,
        int? pageSize)
    {
        var parameters = new SortedDictionary<string, string>();

        if (!string.IsNullOrWhiteSpace(filterCategory))
            parameters.Add("cat", filterCategory.Trim().ToLower());

        if (!string.IsNullOrWhiteSpace(filterProductStatus))
            parameters.Add("stat", filterProductStatus.Trim().ToLower());

        parameters.Add("p", (page ?? 1).ToString());
        parameters.Add("sz", (pageSize ?? 10).ToString());

        if (!string.IsNullOrWhiteSpace(sortColumn))
            parameters.Add("sort", sortColumn.Trim().ToLower());

        if (!string.IsNullOrWhiteSpace(sortOrder))
            parameters.Add("ord", sortOrder.Trim().ToLower());

        
        var rawString = string.Join("|", parameters.Select(kv => $"{kv.Key}={kv.Value}"));

        var hash = Convert.ToHexString(MD5.HashData(Encoding.UTF8.GetBytes(rawString))).ToLower();

        return $"{listPrefix}{hash}";

    }
}
