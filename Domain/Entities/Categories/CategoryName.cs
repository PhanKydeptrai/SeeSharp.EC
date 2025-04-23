using Domain.Primitives;

namespace Domain.Entities.Categories;

public sealed class CategoryName : ValueObject
{
    private CategoryName(string value) => Value = value;
    public string Value { get; }

    public static CategoryName NewCategoryName(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new ArgumentException(nameof(value), "Category name cannot be empty");
        }
        
        if (value.Length > MaxLength)
        {
            throw new ArgumentException(
                nameof(value),
                $"Category name cannot be longer than {MaxLength} characters");
        }
        
        return new CategoryName(value);
    }
    public static CategoryName FromString(string value) => NewCategoryName(value);

    public static readonly CategoryName Empty = new(string.Empty);
    private const int MaxLength = 50;
    public override IEnumerable<object> GetAtomicValues()
    {
        yield return Value;
    }
}
