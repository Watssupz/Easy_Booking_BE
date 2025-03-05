namespace Easy_Booking_BE.Models;

public class MediaModel
{
    public int media_id { get; set; }
    public int room_id { get; set; }
    public byte[]? picture  { get; set; }
}