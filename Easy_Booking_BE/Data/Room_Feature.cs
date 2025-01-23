using System.ComponentModel.DataAnnotations.Schema;

namespace EasyBooking.Data;

[Table("Room_Feature")]
public class Room_Feature
{
    [ForeignKey("Room")]
    public int room_id { get; set; }
    [ForeignKey("Feature")]
    public int feature_id { get; set; }
    
    public virtual Room Room { get; set; }
    public virtual Feature Feature { get; set; }
    
}