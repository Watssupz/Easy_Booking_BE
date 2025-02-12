using System.ComponentModel.DataAnnotations;

namespace Easy_Booking_BE.Models;

public class LocationModel
{
    public int location_id { get; set; }
    [Required]
    public string location_name { get; set; }
}