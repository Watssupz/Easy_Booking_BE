using System.ComponentModel.DataAnnotations;

namespace Easy_Booking_BE.Models
{
    public class SignInModel
    {
        [Required, EmailAddress]
        public string email { get; set; }

        [Required]
        public string password { get; set; }

    }
}
