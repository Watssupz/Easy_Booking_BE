using Easy_Booking_BE.Models;
using Easy_Booking_BE.Models.Response;
using Microsoft.AspNetCore.Identity;

namespace Easy_Booking_BE.Repositories
{
    public interface IAccountRepository
    {
        public Task<BaseDataResponse<IdentityResult>> SignUpAsync(SignUpModel model);
        public Task<BaseDataResponse<string>> SignInAsync(SignInModel model);

    }
}
