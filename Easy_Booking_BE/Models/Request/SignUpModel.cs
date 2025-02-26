using System.ComponentModel.DataAnnotations;

namespace Easy_Booking_BE.Models
{
    public class SignUpModel
    {
        [Required]
        public string first_name { get; set; }
        [Required]
        public string last_name { get; set; }
        [Required, EmailAddress]
        public string email { get; set; }
        [Required]
        public string password { get; set; }
        [Required]
        public string confirm_password { get; set; }
        [RegularExpression(@"^0\d{9}$")]
        public string phone_number { get; set; }
    }
}
