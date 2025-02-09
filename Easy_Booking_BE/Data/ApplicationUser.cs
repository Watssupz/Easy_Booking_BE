using Microsoft.AspNetCore.Identity;

namespace Easy_Booking_BE.Data
{
    public class ApplicationUser : IdentityUser
    {
        public string first_name { get; set; }
        public string last_name { get; set; }


    }
}
