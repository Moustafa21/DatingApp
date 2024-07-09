using System.Security.Claims;

namespace API;

public static class ClaimsPrincipalExtension
{
  public static string GetUsername(this ClaimsPrincipal user)
    {
        var username = user.FindFirstValue(ClaimTypes.NameIdentifier)
                ?? throw new Exception("Cannot get user");

        
        return username;
    }
}
