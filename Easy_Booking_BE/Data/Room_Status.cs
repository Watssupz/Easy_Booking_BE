using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EasyBooking.Data;


[Table("Room_Status")]
public class Room_Status
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int room_status_id { get; set; }
    public string room_status_name { get; set; }
}