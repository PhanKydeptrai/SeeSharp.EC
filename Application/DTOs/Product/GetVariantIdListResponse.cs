namespace Application.DTOs.Product;

public record GetVariantIdListResponse(
    List<Guid> VariantIds, 
    int Page, 
    int PageSize, 
    int TotalCount);
