namespace SharedKernel.Constants;

public static class EndpointTags
{
    public const string Category = "Category";
    public const string Product = "Product";
    public const string Customer = "Customer";
    public const string Order = "Order";
    public const string Wishlist = "Wishlist";
}

public static class EndpointName
{
    public class Customer
    {
        public const string SignUp = "SignUp";
        public const string SignIn = "SignIn";
        public const string SignInWithGoogle = "SignInWithGoogle";
        public const string RevokeRefreshToken = "RevokeRefreshTokenForCustomer";
        public const string RevokeRefreshTokens = "RevokeAllRefreshTokenForCustomer";
        public const string SignInWithRefreshToken = "CustomerSignInWithRefreshToken";
        public const string Verify = "Verify";
        public const string ChangePasswordConfirm = "ChangePasswordConfirm";
        public const string ResetPasswordConfirm = "ResetPasswordConfirm";
        public const string ResetPassword = "ResetPassword";
        public const string ChangePassword = "ChangePassword";
        public const string GetProfile = "GetProfile";

        // public const string SignIn = "SignIn";
        // public const string SignOut = "SignOut";
        // public const string GetProfile = "GetProfile";
        // public const string UpdateProfile = "UpdateProfile";
        // public const string ChangePassword = "ChangePassword";
        // public const string ForgotPassword = "ForgotPassword";
        // public const string ResetPassword = "ResetPassword";
        // public const string ConfirmEmail = "ConfirmEmail";
        // public const string ResendEmailConfirmation = "ResendEmailConfirmation";
    }
    
    public class Employee
    {
        public const string GetAll = "GetAllEmployees";
        public const string GetById = "GetEmployeeById";
        public const string Create = "CreateEmployee";
        public const string Update = "UpdateEmployee";
        public const string UpdateStatus = "UpdateEmployeeStatus";
        public const string ResetPassword = "EmployeeResetPassword";
        public const string ChangePassword = "EmployeeChangePassword";
        public const string GetProfile = "GetEmployeeProfile";
        public const string SignIn = "EmployeeSignIn";
        public const string SignInWithRefreshToken = "EmployeeSignInWithRefreshToken";
    }
    
    public class Product
    {
        public const string Create = "CreateProduct";
        public const string GetById = "GetProductById";
        public const string GetAll = "GetProducts";
        public const string GetAllVariant = "GetAllProductVariant";
        public const string Update = "UpdateProduct";
        public const string Delete = "DeleteProduct";
        public const string Restore = "RestoreProduct";
        public const string GetVariantById = "GetVariantById";
        public const string DeleteVariant = "DeleteVariant";
        public const string RestoreVariant = "RestoreVariant";
        public const string UpdateVariant = "UpdateVariant";
        public const string CreateVariant = "CreateVariant";
    }

    public class Category
    {
        public const string Create = "CreateCategory";
        public const string GetById = "GetCategoryById";
        public const string GetByIdForAdmin = "GetCategoryByIdForAdmin";
        public const string GetAll = "GetCategories";
        public const string Update = "UpdateCategory";
        public const string Delete = "DeleteCategory";
        public const string Restore = "RestoreCategory";
    }

    public class Order
    {
        public const string AddProductToOrder = "AddProductToOrder";
        public const string UpdateOrderDetail = "UpdateOrderDetail";
        public const string GetById = "GetOrder";
        public const string DeleteOrderDetail = "DeleteOrderDetail";
        public const string GetOrderByOrderId = "GetOrderByOrderId";
        public const string GetCartInformation = "GetCartInformation";
        public const string GetAllOrderForAdmin = "GetAllOrderForAdmin";
        public const string GetAllOrderForCustomer = "GetAllOrderForCustomer";
    }

    public class Wishlist
    {
        public const string AddWishList = "AddWishList";
        public const string RemoveWishList = "RemoveWishList";
        public const string GetWishList = "GetWishList";
    }
}

public static class EndpointMethod
{
    public const string POST = "POST";
    public const string GET = "GET";
    public const string PUT = "PUT";
    public const string DELETE = "DELETE";
}

public static class CustomJwtRegisteredClaimNames
{
    public const string CustomerId = "CustomerId";
}

