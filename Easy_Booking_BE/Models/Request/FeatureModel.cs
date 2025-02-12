using System.ComponentModel.DataAnnotations;

namespace Easy_Booking_BE.Models;

public class FeatureModel
{
    public int feature_id { get; set; }
    [Required]
    public string feature_name { get; set; }
}