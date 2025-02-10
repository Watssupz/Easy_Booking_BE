using Easy_Booking_BE.Data;
using Easy_Booking_BE.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Easy_Booking_BE.Models.Response;
using Easy_Booking_BE.Data.Constants;

namespace Easy_Booking_BE.Repositories
{
    public class AccountRepository : IAccountRepository
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IConfiguration _configuration;

        public AccountRepository(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, IConfiguration configuration)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _configuration = configuration;
        }

        public async Task<BaseDataResponse<string>> SignInAsync(SignInModel signInModel)
        {
            var result = await _signInManager.PasswordSignInAsync(signInModel.email, signInModel.password, false, false);
            if(!result.Succeeded)
            {
                return new BaseDataResponse<string>
                    (
                        statusCode: 400, 
                        message: Constants.ERROR, 
                        data: null
                    );
            }
            var authenticationClaims = new List<Claim>
            {
                new Claim(ClaimTypes.Email, signInModel.email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var authenticationKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]));

            var token = new JwtSecurityToken(
                issuer: _configuration["JWT:ValidIssuer"],
                audience: _configuration["JWT:ValidAudience"],
                expires: DateTime.Now.AddMinutes(30),
                claims: authenticationClaims,
                signingCredentials: new SigningCredentials(authenticationKey, SecurityAlgorithms.HmacSha512Signature)
            );
            
            var tokenString = new JwtSecurityTokenHandler().WriteToken(token);
            return new BaseDataResponse<string>
                (
                    statusCode: 200,
                    message: Constants.SUCCESSFUL,
                    data: tokenString
                );

        }

        public async Task<BaseDataResponse<IdentityResult>> SignUpAsync(SignUpModel signUpModel)
        {
            var user = new ApplicationUser
            {
                first_name = signUpModel.first_name,
                last_name = signUpModel.last_name,
                Email = signUpModel.email,
                UserName = signUpModel.email,
            };
            var result = await _userManager.CreateAsync(user, signUpModel.password);
            return new BaseDataResponse<IdentityResult>
            (
                statusCode: result.Succeeded ? 200 : 400,
                message: result.Succeeded ? Constants.SUCCESSFUL : Constants.ERROR
            );
        }
    }
}
