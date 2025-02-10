using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Easy_Booking_BE.Models;

public class Payment_StatusModel
{
    public int payment_id { get; set; }
    [Required]
    public string payment_status_name { get; set; }
}