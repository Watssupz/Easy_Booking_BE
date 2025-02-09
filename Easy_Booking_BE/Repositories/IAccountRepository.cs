using Easy_Booking_BE.Models;
using Microsoft.AspNetCore.Identity;

namespace Easy_Booking_BE.Repositories
{
    public interface IAccountRepository
    {
        public Task<IdentityResult> SignUpAsync(SignUpModel model);
        public Task<string> SignInAsync(SignInModel model);

    }
}
