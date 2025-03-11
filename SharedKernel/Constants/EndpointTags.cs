namespace SharedKernel.Constants;

public static class EndpointTag
{
    public const string Category = "Category";
    public const string Product = "Product";
    public const string Customer = "Customer";
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
        public const string Restore = "RestoreCategory";
    }
}

public static class EndpointMethod
{
    public const string POST = "POST";
    public const string GET = "GET";
    public const string PUT = "PUT";
    public const string DELETE = "DELETE";
}

