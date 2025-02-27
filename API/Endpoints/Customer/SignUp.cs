
using MediatR;

namespace API.Endpoints.Customer;

internal sealed class SignUp : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("api/customer/signup", async (
            
            ISender sender) =>
        {
            
        }); 
    }
}
