namespace Application.DTOs.Category;

public record GetCategoryIdListResponse(
    List<Guid> CategoryIds, 
    int Page, 
    int PageSize, 
    int TotalCount);
