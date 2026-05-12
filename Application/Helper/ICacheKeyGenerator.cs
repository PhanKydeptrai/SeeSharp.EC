namespace Infrastructure.Helper;

public interface ICacheKeyGenerator
{
    /// <summary>
    /// Hàm này tạo ra một cache key "dễ đọc" dựa trên các tham số lọc, sắp xếp và phân trang. 
    /// Mục đích là để có thể dễ dàng debug và quản lý cache keys trong Redis, thay vì sử dụng một chuỗi hash khó hiểu.
    /// </summary>
    /// <param name="keyPrefix"></param>
    /// <param name="filterProductStatus"></param>
    /// <param name="filterCategory"></param>
    /// <param name="sortColumn"></param>
    /// <param name="sortOrder"></param>
    /// <param name="page"></param>
    /// <param name="pageSize"></param>
    /// <param name="filterProduct"></param>
    /// <returns></returns>
    Task<string> CreateCacheKeyAsync(
        string keyPrefix,
        string? filterProductStatus,
        string? filterCategory,
        string? sortColumn,
        string? sortOrder,
        int? page,
        int? pageSize,
        string? filterProduct = null);

}
