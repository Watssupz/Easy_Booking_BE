using System.ComponentModel.DataAnnotations;

namespace Easy_Booking_BE.Models;

public class Booking_StatusModel
{
    public int booking_status_id { get; set; }
    [Required]
    public string booking_status_name { get; set; }
}