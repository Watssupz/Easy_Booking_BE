namespace Easy_Booking_BE.Models;

public class Room_FeatureIdsModel
{
    public RoomModel Room { get; set; }
    public List<int>? FeatureIds { get; set; }
}