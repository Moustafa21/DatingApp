using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using API.Entities;
using API.Interfaces;
using Microsoft.IdentityModel.Tokens;

namespace API.Services;

public class TokenService(IConfiguration config) : ITokenService
{
    public string CreateToken(AppUser user)
    {
        var tokenkey = config["TokenKey"] ?? throw new Exception("Cannot access token");
        if (tokenkey.Length<64)
            throw new Exception("your token need to be longer");
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(tokenkey));
        var claims = new List<Claim>
        {
            new (ClaimTypes.NameIdentifier, user.UserName),
        };

        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);
        var tokenDesciptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.AddDays(1),
            SigningCredentials = creds

        };

        var tokenHandler = new JwtSecurityTokenHandler();

        var token = tokenHandler.CreateToken(tokenDesciptor);
        return tokenHandler.WriteToken(token);
    }
}
