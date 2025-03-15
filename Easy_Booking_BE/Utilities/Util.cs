using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;

namespace Easy_Booking_BE.Utilities;

public class Util
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public Util(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }
    
    public async Task<string> GetTokenAsync()
    {
        var token = await _httpContextAccessor.HttpContext.GetTokenAsync("Bearer", "access_token");
        if (string.IsNullOrEmpty(token))
        {
            throw new Exception("Token not found.");
        }
        return token;
    }
    
    // Get user_id from token
    public async Task<string> GetUserIdFromTokenAsync()
    {
        var token = await _httpContextAccessor.HttpContext.GetTokenAsync("Bearer", "access_token");
        if (string.IsNullOrEmpty(token))
        {
            throw new Exception("Token not found.");
        }

        var handler = new JwtSecurityTokenHandler();
        var jwtToken = handler.ReadJwtToken(token);

        var userId = jwtToken.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;

        if (string.IsNullOrEmpty(userId))
        {
            throw new Exception("User ID not found in token.");
        }
        return userId;
    }
    
    public string GetBase64WithoutPrefix(string base64String)
    {
        if (string.IsNullOrEmpty(base64String))
        {
            return null;
        }
        if (base64String.Contains(";base64,"))
        {
            return base64String.Split(',')[1];
        }
        return base64String;
    }
}