namespace API.Extentions;

public class ApiResponse<T>
{
    public int Status { get; set; }
    public string Message { get; set; } = string.Empty;
    public object Data { get; set; } = default!;
}

public enum ApiResponseStatus
{
    Success = 200,
    Created = 201,
    NoContent = 204,
    BadRequest = 400,
    Unauthorized = 401,
    Forbidden = 403,
    NotFound = 404,
    InternalServerError = 500
}
