namespace Easy_Booking_BE.Models;

public class UserModel
{
    public string? first_name { get; set; }
    public string? last_name { get; set; }
    public string? email { get; set; }
    public string? phone_number { get; set; }
    public List<string>? roles { get; set; }
    public string? avatar { get; set; }
}