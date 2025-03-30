using System.Drawing;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Easy_Booking_BE.Models;
using Final_Project_PRN221.Models.DTO;
using Microsoft.AspNetCore.Authentication;
using Newtonsoft.Json;
using RestSharp;

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
    
    // bank
    public static Bank ReadJsonFile(string filePath)
    {
        using StreamReader reader = new StreamReader(filePath);
        string jsonContent = reader.ReadToEnd();
        return JsonConvert.DeserializeObject<Bank>(jsonContent);
    }
    
    public static Image Base64ToImage(string base64String)
    {
        byte[] imageBytes = Convert.FromBase64String(base64String);
        MemoryStream ms = new MemoryStream(imageBytes, 0, imageBytes.Length);
        ms.Write(imageBytes, 0, imageBytes.Length);
        System.Drawing.Image image = System.Drawing.Image.FromStream(ms, true);
        return image;
    }
    
    public static async Task<string> GenQR(int acpId, string accountNo, string accountName, int amount, string format, string template, string addInfor)
    {
        var apiRequest = new ApiRequest();
        apiRequest.acqId = acpId;
        apiRequest.accountNo = accountNo;
        apiRequest.accountName = accountName;
        apiRequest.amount = amount;
        apiRequest.format = format;
        apiRequest.template = template;
        apiRequest.addInfo = addInfor;
        var jsonRequest = JsonConvert.SerializeObject(apiRequest);
        var client = new RestClient("https://api.vietqr.io/v2/generate");
        var request = new RestRequest();

        request.Method = Method.Post;
        request.AddHeader("Accept", "application/json");
        request.AddParameter("application/json", jsonRequest, ParameterType.RequestBody);

        var response = await client.ExecuteAsync(request);
        var content = response.Content;
    
        var dataResult = JsonConvert.DeserializeObject<ApiRequest.ApiResponse>(content);
        var image = Base64ToImage(dataResult.data.qrDataURL.Replace("data:image/png;base64,", ""));

        return dataResult.data.qrDataURL;
    }
}