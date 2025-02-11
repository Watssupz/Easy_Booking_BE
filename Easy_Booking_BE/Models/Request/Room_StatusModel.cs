using System.ComponentModel.DataAnnotations;

namespace Easy_Booking_BE.Models;

public class Room_StatusModel
{
    public int room_status_id { get; set; }
    [Required]
    public string room_status_name { get; set; }
}