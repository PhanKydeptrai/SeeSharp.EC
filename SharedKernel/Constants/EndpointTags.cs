namespace SharedKernel.Constants;

public static class EndpointName
{
    public class Product
    {
        public const string Create = "CreateProduct";
        public const string GetById = "GetProductById";
        public const string GetAll = "GetProducts";
        public const string Update = "UpdateProduct";
        public const string Delete = "DeleteProduct";
        public const string Restore = "RestoreProduct";
    }   

    public class Category
    {
        public const string Create = "CreateCategory";
        public const string GetById = "GetCategoryById";
        public const string GetAll = "GetCategories";
        public const string Update = "UpdateCategory";
        public const string Delete = "DeleteCategory";
    }
}

public static class EndpointMethod
{
    public const string POST = "POST";
    public const string GET = "GET";
    public const string PUT = "PUT";
    public const string DELETE = "DELETE";
}

public static class EndpointTag
{
    public const string Category = "Category";
    public const string Product = "Product";
}