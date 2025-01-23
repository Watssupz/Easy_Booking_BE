using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EasyBooking.Data;

[Table("Feature")]
public class Feature
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int feature_id { get; set; }
    public int feature_name { get; set; }
}