using System.ComponentModel.DataAnnotations;

namespace Easy_Booking_BE.Models;

public class UpdateRoomFeatureModel
{
    public int room_id { get; set; }
    [Required]
    public List<int> FeatureIds { get; set; }
}