using Easy_Booking_BE.Data.Constants;
using Easy_Booking_BE.Models;
using Easy_Booking_BE.Models.Response;
using Easy_Booking_BE.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Easy_Booking_BE.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ACController : ControllerBase
    {
        private readonly IAccountRepository _accountRepo;

        public ACController(IAccountRepository repo)
        {
            _accountRepo = repo;
        }

        [HttpPost("SignUp")]
        public async Task<IActionResult> SignUp(SignUpModel model)
        {
            if(model.password != model.confirm_password)
            {
                return BadRequest(
                    new BaseDataResponse<object>
                    (
                        statusCode: 400,
                        message: Constants.ERROR
                    )
                );
            }

            var result = await _accountRepo.SignUpAsync(model);
            return result.StatusCode == 200 ? Ok(result) : BadRequest(result);
        }

        [HttpPost("SignIn")]
        public async Task<IActionResult> SignIn(SignInModel model)
        {
            var response = await _accountRepo.SignInAsync(model);
            if (response.StatusCode == 400)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }

        [Authorize]
        [HttpPost("UpdateProf")]
        public async Task<IActionResult> UpdateProfile(UserModel model)
        {
            var response = await _accountRepo.UpdateProfile(model);
            return response.StatusCode == 200 ? Ok(response) : BadRequest(response);
        }

        [Authorize]
        [HttpPost("ChgPwd")]
        public async Task<IActionResult> UpdatePassword(PasswordModel model)
        {
            var response = await _accountRepo.UpdatePassword(model);
            return response.StatusCode == 200 ? Ok(response) : BadRequest(response);
        }
    }
}
