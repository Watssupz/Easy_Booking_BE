using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using EasyBooking.Data;

namespace Easy_Booking_BE.Data;
[Table("Media")]
public class Media
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int media_id { get; set; }
    
    [ForeignKey("Room")]
    public int room_id { get; set; }
    public byte[]? picture  { get; set; }
    
    public virtual Room Room { get; set; }
}