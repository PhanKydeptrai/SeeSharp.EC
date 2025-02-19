namespace SharedKernel.Constants;

public static class EndpointConstants
{
    public static class Name
    {
        
    }

    /// <summary>
    /// The Tag class provides a collection of constants representing
    /// various endpoint tags in the application.
    /// </summary>
    /// <remarks>
    /// These constants are typically used for identifying specific endpoint
    /// categories in routing or documentation purposes.
    /// </remarks>
    /// <example>
    /// This class should not be instantiated directly.
    /// Use the constants provided as needed.
    /// </example>
    public static class Tag 
    {
        public const string Bill = "Bill";
        public const string Category = "Customer";
        public const string Customer = "Category";
        public const string Product = "Product";
        public const string Order = "Order";
        public const string OrderDetails = "OrderDetail";
        public const string OrderTransaction = "OrderTransaction";
        public const string User = "User";
        public const string ShippingInformation = "ShippingInformation";
        public const string PaymentInformation = "PaymentInformation";
        public const string UserAuthenticationTokens = "UserAuthenticationTokens";
        public const string VerificationTokens = "VerificationTokens";
        public const string Voucher = "Voucher";
        public const string WishItem = "WishItem";
    }

    
    
}