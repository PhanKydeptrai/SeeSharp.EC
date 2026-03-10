using System.IdentityModel.Tokens.Jwt;
using Application.Security;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.DependencyInjection;
using SharedKernel.Constants;

namespace Infrastructure.Security;

public class CustomJwtBearerEvents : JwtBearerEvents
{
    public override async Task TokenValidated(TokenValidatedContext context)
    {
        var jti = context.Principal?.FindFirst(JwtRegisteredClaimNames.Jti)?.Value;
        var isGuest = context.Principal?.FindFirst(CustomJwtRegisteredClaimNames.GuestId)?.Value ?? string.Empty;

        if (jti is not null && isGuest == string.Empty)
        {
            var revocationService = context.HttpContext.RequestServices
                .GetRequiredService<ITokenRevocationService>();

            if (await revocationService.IsTokenRevoked(jti))
            {
                // Thu hồi tất cả token trong chain 
                await revocationService.RevokeTokenChainByJtiAsync(jti);
                
                context.Fail("Token was revoked, and the entire token chain has been compromised and invalidated.");
            }
        }
        
        await base.TokenValidated(context);
    }
}
