using Domain.Primitives;

namespace Domain.Entities.Categories;

public sealed class CategoryId : BaseId<CategoryId>
{
    public static readonly CategoryId DefaultCategoryId = FromGuid(new Guid("019546cc-2909-1710-9a1b-36df36d9a7ae"));
}

