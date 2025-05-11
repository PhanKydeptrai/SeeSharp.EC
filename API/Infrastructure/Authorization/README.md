# Role-Based Authentication for Minimal APIs

Tài liệu hướng dẫn cách sử dụng xác thực và phân quyền trong Minimal API.

## Các vai trò được định nghĩa

Hệ thống hiện đang hỗ trợ các vai trò sau:

- `Admin`: Quản trị viên hệ thống
- `Employee`: Nhân viên (bao gồm cả Admin)
- `Subscribed`: Khách hàng đã đăng ký
- `Guest`: Khách vãng lai

## Các chính sách phân quyền

Các chính sách phân quyền được định nghĩa trong class `AuthorizationPolicies`:

- `AdminOnly`: Chỉ Admin mới có quyền truy cập
- `AllEmployee`: Employee và Admin có quyền truy cập
- `SubscribedOnly`: Chỉ khách hàng đã đăng ký mới có quyền truy cập
- `Guest`: Chỉ khách vãng lai mới có quyền truy cập
- `SubscribedOrGuest`: Khách hàng đã đăng ký hoặc khách vãng lai đều có quyền truy cập

## Cách sử dụng

### 1. Áp dụng phân quyền cho từng endpoint

```csharp
app.MapPost("api/products", async (/* parameters */) => 
{
    // Xử lý logic
})
.WithTags("Products")
.RequireAdmin(); // Chỉ Admin mới có quyền truy cập
```

### 2. Áp dụng phân quyền cho nhóm endpoint

```csharp
// Nhóm endpoint chỉ dành cho Admin
app.MapAdminGroup("api/admin", group =>
{
    group.MapGet("dashboard", () => Results.Ok(new { message = "Admin Dashboard" }));
    group.MapPost("settings", () => Results.NoContent());
});

// Nhóm endpoint dành cho Employee (bao gồm cả Admin)
app.MapEmployeeGroup("api/employee", group =>
{
    group.MapGet("tasks", () => Results.Ok(new { message = "Employee Tasks" }));
    group.MapPost("reports", () => Results.NoContent());
});
```

### 3. Áp dụng phân quyền với vai trò tùy chỉnh

```csharp
app.MapPost("api/custom", async (/* parameters */) => 
{
    // Xử lý logic
})
.WithTags("Custom")
.RequireRoles("Admin", "CustomRole"); // Yêu cầu vai trò Admin hoặc CustomRole
```

## Các extension method hỗ trợ

### Cho từng endpoint

- `RequireAdmin()`: Yêu cầu quyền Admin
- `RequireEmployee()`: Yêu cầu quyền Employee hoặc Admin
- `RequireSubscribed()`: Yêu cầu quyền Subscribed
- `RequireGuest()`: Yêu cầu quyền Guest
- `RequireSubscribedOrGuest()`: Yêu cầu quyền Subscribed hoặc Guest
- `RequireRoles(params string[] roles)`: Yêu cầu một trong các vai trò được chỉ định

### Cho nhóm endpoint

- `MapAdminGroup(string prefix, Action<RouteGroupBuilder> configure)`: Tạo nhóm endpoint chỉ dành cho Admin
- `MapEmployeeGroup(string prefix, Action<RouteGroupBuilder> configure)`: Tạo nhóm endpoint dành cho Employee
- `MapSubscribedGroup(string prefix, Action<RouteGroupBuilder> configure)`: Tạo nhóm endpoint dành cho khách hàng đã đăng ký
- `MapGuestGroup(string prefix, Action<RouteGroupBuilder> configure)`: Tạo nhóm endpoint dành cho khách vãng lai
- `MapSubscribedOrGuestGroup(string prefix, Action<RouteGroupBuilder> configure)`: Tạo nhóm endpoint dành cho khách hàng đã đăng ký hoặc khách vãng lai
- `MapRoleGroup(string prefix, string[] roles, Action<RouteGroupBuilder> configure)`: Tạo nhóm endpoint dành cho các vai trò được chỉ định 