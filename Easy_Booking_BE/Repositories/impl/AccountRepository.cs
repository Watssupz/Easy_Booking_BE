using Easy_Booking_BE.Data;
using Easy_Booking_BE.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Easy_Booking_BE.Models.Response;
using Easy_Booking_BE.Data.Constants;
using Easy_Booking_BE.Utilities;

namespace Easy_Booking_BE.Repositories
{
    public class AccountRepository : IAccountRepository
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IConfiguration _configuration;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly Util _util;

        public AccountRepository(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager,
            IConfiguration configuration, RoleManager<IdentityRole> roleManager, Util util)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _configuration = configuration;
            _roleManager = roleManager;
            _util = util;
        }

        public async Task<BaseDataResponse<string>> SignInAsync(SignInModel signInModel)
        {
            var user = await _userManager.FindByEmailAsync(signInModel.email);
            var passwordValid = await _userManager.CheckPasswordAsync(user, signInModel.password);
            if (user == null || !passwordValid)
            {
                return new BaseDataResponse<string>
                (
                    statusCode: 400,
                    message: Constants.ERROR
                );
            }

            var authenticationClaims = new List<Claim>
            {
                new Claim(ClaimTypes.Email, signInModel.email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.NameIdentifier, user.Id)
            };

            var userRole = await _userManager.GetRolesAsync(user);
            foreach (var role in userRole)
            {
                authenticationClaims.Add(new Claim(ClaimTypes.Role, role));
            }

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
                UserName = signUpModel.first_name + " " + signUpModel.last_name,
                PhoneNumber = signUpModel.phone_number
            };
            var result = await _userManager.CreateAsync(user, signUpModel.password);

            if (result.Succeeded)
            {
                if (!await _roleManager.RoleExistsAsync(Constants.CUSTOMER))
                {
                    await _roleManager.CreateAsync(new IdentityRole(Constants.CUSTOMER));
                }

                await _userManager.AddToRoleAsync(user, Constants.CUSTOMER);
            }

            return new BaseDataResponse<IdentityResult>
            (
                statusCode: result.Succeeded ? 200 : 400,
                message: result.Succeeded ? Constants.SUCCESSFUL : Constants.ERROR
            );
        }

        public async Task<BaseDataResponse<string>> UpdateProfile(UserModel model)
        {
            var token = await _util.GetTokenAsync();
            //decode token
            var handler = new JwtSecurityTokenHandler();
            var jwtToken = handler.ReadJwtToken(token);
            // get userid from token
            var userId = jwtToken.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userId))
            {
                return new BaseDataResponse<string>
                (
                    statusCode: 404,
                    message: Constants.NOT_FOUND
                );
            }

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return new BaseDataResponse<string>(
                    statusCode: 404,
                    message: "User " + Constants.NOT_FOUND
                );
            }

            if (!string.IsNullOrEmpty(model.first_name))
            {
                user.first_name = model.first_name;
            }

            if (!string.IsNullOrEmpty(model.last_name))
            {
                user.last_name = model.last_name;
            }

            if (!string.IsNullOrEmpty(model.phone_number))
            {
                user.PhoneNumber = model.phone_number;
            }

            if (!string.IsNullOrEmpty(model.email))
            {
                user.Email = model.email;
            }

            if (string.IsNullOrEmpty((model.first_name)) && (string.IsNullOrEmpty(model.last_name)) &&
                (string.IsNullOrEmpty(model.phone_number)) && (string.IsNullOrEmpty(model.email)))
            {
                return new BaseDataResponse<string>(
                    statusCode: 400,
                    message: Constants.NO_FIELDS_FOR_UPDATE
                );
            }

            var result = await _userManager.UpdateAsync(user);
            if (!result.Succeeded)
            {
                return new BaseDataResponse<string>(
                    statusCode: 400,
                    message: Constants.UPDATE_ERROR
                );
            }

            return new BaseDataResponse<string>(
                statusCode: 200,
                message: Constants.UPDATE_SUCCESS
            );
        }

        public async Task<BaseDataResponse<string>> UpdatePassword(PasswordModel model)
        {
            var token = await _util.GetTokenAsync();
            //decode token
            var handler = new JwtSecurityTokenHandler();
            var jwtToken = handler.ReadJwtToken(token);
            // get userid from token
            var userId = jwtToken.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
            {
                return new BaseDataResponse<string>
                (
                    statusCode: 404,
                    message: Constants.NOT_FOUND
                );
            }

            // Kiểm tra xem model có chứa thông tin mật khẩu không
            if (model == null || string.IsNullOrEmpty(model.current_password) ||
                string.IsNullOrEmpty(model.new_password))
            {
                return new BaseDataResponse<string>(
                    statusCode: 400,
                    message: "Current password and new password are required."
                );
            }

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return new BaseDataResponse<string>(
                    statusCode: 404,
                    message: "User " + Constants.NOT_FOUND
                );
            }

            // Xác minh mật khẩu cũ (nếu cần)
            var isPasswordValid = await _userManager.CheckPasswordAsync(user, model.current_password);
            if (!isPasswordValid)
            {
                return new BaseDataResponse<string>(
                    statusCode: 400,
                    message: "Current password is incorrect."
                );
            }

            // Cập nhật mật khẩu mới
            var result = await _userManager.ChangePasswordAsync(user, model.current_password, model.new_password);
            if (!result.Succeeded)
            {
                return new BaseDataResponse<string>(
                    statusCode: 400,
                    message: Constants.UPDATE_ERROR
                );
            }

            return new BaseDataResponse<string>(
                statusCode: 200,
                message: Constants.UPDATE_SUCCESS
            );
        }

        public async Task<BaseDataResponse<UserModel>> GetUser()
        {
            var token = await _util.GetTokenAsync();
            //decode token
            var handler = new JwtSecurityTokenHandler();
            var jwtToken = handler.ReadJwtToken(token);
            // get userid from token
            var userId = jwtToken.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
            {
                return new BaseDataResponse<UserModel>
                (
                    statusCode: 404,
                    message: Constants.NOT_FOUND
                );
            }

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return new BaseDataResponse<UserModel>(
                    statusCode: 404,
                    message: "User " + Constants.NOT_FOUND
                );
            }
            
            var roles = jwtToken.Claims
                .Where(c => c.Type == ClaimTypes.Role)
                .Select(c => c.Value)
                .ToList();

            var userModel = new UserModel
            {
                first_name = user.first_name,
                last_name = user.last_name,
                email = user.Email,
                phone_number = user.PhoneNumber,
                roles = roles
            };
            return new BaseDataResponse<UserModel>(
                statusCode: 200,
                message: Constants.SUCCESSFUL,
                data: userModel
            );
        }

        public async Task<BaseDataResponse<string>> CreateHostByEmail(UserModel model)
        {
            try
            {
                if (string.IsNullOrEmpty(model.email))
                {
                    return new BaseDataResponse<string>(
                        statusCode: 404,
                        message: "Email is required"
                    );
                }

                var user = await _userManager.FindByEmailAsync(model.email);
                if (user == null)
                {
                    return new BaseDataResponse<string>(
                        statusCode: 404,
                        message: Constants.NOT_FOUND
                    );
                }

                var roles = await _userManager.GetRolesAsync(user);
                if (roles.Contains(Constants.HOST))
                {
                    return new BaseDataResponse<string>(
                        statusCode: 400,
                        message: Constants.ALREADY_EXSIST
                    );
                }

                var result = await _userManager.AddToRoleAsync(user, Constants.HOST);
                if (!result.Succeeded)
                {
                    return new BaseDataResponse<string>
                    (
                        statusCode: 400,
                        message: Constants.ERROR
                    );
                }

                return new BaseDataResponse<string>(
                    statusCode: 200,
                    message: Constants.SUCCESSFUL
                );
            }
            catch (Exception e)
            {
                return new BaseDataResponse<string>
                (
                    statusCode: 500,
                    message: Constants.ERROR
                );
            }
        }
    }
}