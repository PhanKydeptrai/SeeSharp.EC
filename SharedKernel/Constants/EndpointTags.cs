namespace SharedKernel.Constants;

public static class EndpointTags
{
    public const string Categories = "Categories";
    public const string Product = "Products";
    public const string Customer = "Customer";
    public const string Order = "Order";
    public const string Wishlist = "Wishlist";
    public const string Voucher = "Voucher";
    public const string Employee = "Employee";
    public const string Bills = "Bills";
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
        public const string UpdateProfile = "UpdateProfile";

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
        public const string UpdateProfile = "UpdateEmployeeProfile";
    }
    
    public class Product
    {
        public const string CreateProduct = "CreateProduct";
        public const string GetProductById = "GetProductById";
        public const string GetAll = "GetProducts";
        public const string GetAllVariant = "GetAllProductVariant";
        public const string Update = "UpdateProduct";
        public const string DeleteProduct = "DeleteProduct";
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
        public const string GetCategoryInfo = "GetCategoryInfo";
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
        public const string GetMakePaymentResponse = "GetMakePaymentResponse";
    }

    public class Wishlist
    {
        public const string AddWishList = "AddWishList";
        public const string DeleteWishListItem = "RemoveWishList";
        public const string GetWishList = "GetWishList";
    }

    public class Voucher
    {
        public const string Create = "CreateVoucher";
        public const string GetById = "GetVoucherById";
        public const string GetAll = "GetVouchers";
        public const string Update = "UpdateVoucher";
        public const string Delete = "DeleteVoucher";
        public const string GetAllForCustomer = "GetAllCustomerVouchers";
    }

    public class Bill
    {
        public const string GetAll = "GetBills";
        public const string GetById = "GetBillById";
        public const string Create = "CreateBill";
        public const string Update = "UpdateBill";
        public const string Delete = "DeleteBill";
        public const string Restore = "RestoreBill";
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
    public const string EmployeeId = "EmployeeId";
    public const string GuestId = "GuestId";
}

