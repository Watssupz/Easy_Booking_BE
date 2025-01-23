using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EasyBooking.Data;

[Table("Room")]
public class Room
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int room_id { get; set; }
    public string room_number { get; set; }
    public double price_per_night { get; set; }
    public string description { get; set; }
    
    [ForeignKey("Floor")]
    public int floor_id { get; set; }
    [ForeignKey("Room_Status")]
    public int room_status_id { get; set; }
    [ForeignKey("Location")]
    public int location_id { get; set; }
    
    public virtual Floor Floor { get; set; }
    public virtual Room_Status Room_Status { get; set; }
    public virtual Location Location { get; set; }
    
}

