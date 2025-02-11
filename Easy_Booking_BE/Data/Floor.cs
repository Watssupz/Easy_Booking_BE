using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EasyBooking.Data;

[Table("Floor")]
public class Floor
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int floor_id { get; set; }
    public string floor_name { get; set; }
}