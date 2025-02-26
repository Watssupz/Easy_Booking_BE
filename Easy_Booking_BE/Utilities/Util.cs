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
}