using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EasyBooking.Data;

[Table("Booking")]
public class Booking
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int booking_id { get; set; }
    public DateTime start_date_booking { get; set; }
    public DateTime end_date_booking { get; set; }
    public DateTime check_in  { get; set; }
    public DateTime check_out  { get; set; }
    public int num_adults { get; set; }
    public int num_children { get; set; }
    public double price { get; set; }
    
    [ForeignKey("Booking_Status")]
    public int booking_status { get; set; }
    [ForeignKey("Payment_Status")]
    public int payment_status { get; set; }
    
    
    
    public virtual Payment_Status Payment_Status { get; set; }
    public virtual Booking_Status Booking_Status { get; set; }
}