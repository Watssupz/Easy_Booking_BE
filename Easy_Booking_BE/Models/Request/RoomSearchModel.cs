namespace Easy_Booking_BE.Models;

public class RoomSearchModel
{
    public string location { get; set; }
    public double? minPrice { get; set; }
    public double? maxPrice { get; set; }
}