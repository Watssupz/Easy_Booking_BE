using Easy_Booking_BE.Models;
using Easy_Booking_BE.Models.Response;
using Microsoft.AspNetCore.Identity;

namespace Easy_Booking_BE.Repositories
{
    public interface IAccountRepository
    {
        public Task<BaseDataResponse<IdentityResult>> SignUpAsync(SignUpModel model);
        public Task<BaseDataResponse<string>> SignInAsync(SignInModel model);
        public Task<BaseDataResponse<string>> UpdateProfile(UserModel model);
        public Task<BaseDataResponse<string>> UpdatePassword(PasswordModel model);
        public Task<BaseDataResponse<UserModel>> GetUser();
        public Task<BaseDataResponse<string>> CreateHostByEmail(UserModel model);
    }
}
