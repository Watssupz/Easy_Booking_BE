using System.ComponentModel.DataAnnotations;

namespace Easy_Booking_BE.Models;

public class BookingCreateModel
{
    [Required] public int room_id { get; set; }
    [Required] public DateTime start_date_booking { get; set; }
    [Required] public DateTime end_date_booking { get; set; }
    [Required] public int num_adults { get; set; }
    [Required] public int num_children { get; set; }
    [Required] public double price { get; set; }
    [Required] public int payment_status { get; set; }
}