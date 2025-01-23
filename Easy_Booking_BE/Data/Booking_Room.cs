using System.ComponentModel.DataAnnotations.Schema;

namespace EasyBooking.Data;

[Table("Booking_Room")]
public class Booking_Room
{
    [ForeignKey("Booking")]
    public int booking_id { get; set; }
    [ForeignKey("Room")]
    public int room_id { get; set; }
    
    public virtual Booking Booking { get; set; }
    public virtual Room Room { get; set; }
    
}