using System.ComponentModel.DataAnnotations;

namespace Easy_Booking_BE.Models;

public class BookingModel
{
    public int booking_id { get; set; }
    [Required] public DateTime start_date_booking { get; set; }
    [Required] public DateTime end_date_booking { get; set; }
    public DateTime? check_in { get; set; }
    public DateTime? check_out { get; set; }
    [Required] public int num_adults { get; set; }
    [Required] public int num_children { get; set; }
    [Required] public double price { get; set; }
    public string user_id { get; set; }
    public string payment_status { get; set; }
    public string booking_status { get; set; }
}