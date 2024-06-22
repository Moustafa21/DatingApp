using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

namespace API.Extensions;

public static class IdentiyServiceExtension
{
    public static IServiceCollection AddIdentityService(this IServiceCollection services, IConfiguration config)
    {
        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
        .AddJwtBearer(opt =>
        {
            var tokenKey = config["TokenKey"] ?? throw new Exception("Tokenkey not found");
            opt.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(tokenKey)),
                ValidateIssuer = false,
                ValidateAudience = false,

            };
        });
        return services;
    }
}
