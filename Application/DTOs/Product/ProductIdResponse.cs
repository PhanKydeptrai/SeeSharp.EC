namespace Application.DTOs.Product;

public record GetProductIdListResponse(
    List<Guid> ProductIds, 
    int Page, 
    int PageSize, 
    int TotalCount);
