using AutoMapper;
using LibrarySystem.Api.DTOs;
using LibrarySystem.Api.Errors;
using LibrarySystem.Api.Extensions;
using LibrarySystem.Core.Entitties;
using LibrarySystem.Core.Entitties.Identity;
using LibrarySystem.Core.Services.Contract;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Web;
using ForgotPasswordRequest = LibrarySystem.Api.DTOs.ForgotPasswordRequest;
using ResetPasswordRequest = LibrarySystem.Api.DTOs.ResetPasswordRequest;

namespace LibrarySystem.Api.Controllers
{
    public class AppUserController : ApiControllerBase
    {
        private readonly SignInManager<AppUser> _signInManager;
        private readonly UserManager<AppUser> _userManager;
        private readonly ITokenService _tokenService;
        private readonly IMapper _mapper;
        private readonly RoleManager<AppRole> _roleManager;
        private readonly IEmailService _emailService;

        public AppUserController(SignInManager<AppUser> signInManager ,
            UserManager<AppUser> userManager , ITokenService tokenService , IMapper mapper , RoleManager<AppRole> roleManager , IEmailService emailService)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _tokenService = tokenService;
            _mapper = mapper;
            _roleManager = roleManager;
            _emailService = emailService;
        }


        [HttpPost("Register")]
        public async Task<ActionResult<UserDto>> Register(RegisterDto user)
        {
            if (CheckEmailExist(user.Email).Result.Value)
                return BadRequest(new ApiResponse(400, "Email is already exist!"));

            if (user.Role != null && (user.Role.Equals("Admin", StringComparison.OrdinalIgnoreCase) || user.Role.Equals("Librarian", StringComparison.OrdinalIgnoreCase)))
            {
                return BadRequest(new ApiResponse(400, "You cannot register as Admin or Librarian."));
            }

            var newUser = new AppUser()
            {
                DisplayName = user.DisplayName,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                UserName = user.Email.Split("@")[0]
            };

            var result = await _userManager.CreateAsync(newUser, user.Password);

            var role = "User";

            if (!await _roleManager.RoleExistsAsync(role))
            {
                await _roleManager.CreateAsync(new AppRole { Name = role });
            }

            if (!await _userManager.IsInRoleAsync(newUser, role))
            {
                var roleResult = await _userManager.AddToRoleAsync(newUser, role);
                if (!roleResult.Succeeded)
                    return BadRequest(new ApiResponse(400, "Failed to assign role."));
            }

            if (result.Succeeded)
            {
                var returnedUser = new UserDto()
                {
                    DisplayName = user.DisplayName,
                    Email = user.Email,
                    Roles = (await _userManager.GetRolesAsync(newUser)).ToList(),
                    Token = await _tokenService.CreateTokenAsync(newUser, _userManager)
                };

                return Ok(returnedUser);
            }

            return BadRequest(new ApiResponse(400, "Failed to register user."));
        }

        [HttpPost("Login")]
        public async Task<ActionResult<UserDto>> Login (LoginDTO user)
        {
            var User = await _userManager.FindByEmailAsync(user.Email);
            if (User is null)
                return Unauthorized(new ApiResponse(401));
            var Result = await _signInManager.CheckPasswordSignInAsync(User , user.Password ,false);
            if (!Result.Succeeded)
                return BadRequest(new ApiResponse(400 ,"Invalid Data"));
            return Ok(new UserDto()
            {
                DisplayName = User.DisplayName,
                Email = User.Email,
                Roles = (await _userManager.GetRolesAsync(User)).ToList(),
                Token = await _tokenService.CreateTokenAsync(User, _userManager)
            });
                
        }

        [Authorize]
        [HttpGet("GetCurrentUser")]
        public async Task<ActionResult<UserDto>> GetCurrentUser()
        {
            var Email = User.FindFirstValue(ClaimTypes.Email);
            if (Email is null)
                return BadRequest(new ApiResponse(400));
            var appuser = await _userManager.FindByEmailAsync(Email);
            var ReturnedUser = new UserDto()
            {
                Email = Email,
                DisplayName = appuser.DisplayName,
                Token =await _tokenService.CreateTokenAsync(appuser, _userManager)
            };
            return Ok(ReturnedUser);

        }

        [Authorize]
        [HttpGet("GetCurrentUserAddress")]
        public async Task<ActionResult<AddressDto>> GetCurrentUserAddress()
        {
            var user =await _userManager.FindUserWithAddressAsync(User);
            var ReturnedAddress = _mapper.Map<Address, AddressDto>(user.Address);
            return Ok(ReturnedAddress);

        }

        [Authorize]
        [HttpPut("UpdateAddress")]
        public async Task<ActionResult<AddressDto>> UpdateAddress(AddressDto address)
        {
          var appuser =   await _userManager.FindUserWithAddressAsync(User);
            if (appuser is null)
                return BadRequest(new ApiResponse(400));

            var Mappedaddress =  _mapper.Map<AddressDto, Address>(address);
            Mappedaddress.Id = appuser.Address.Id;
          appuser.Address = Mappedaddress;

            var Result = await _userManager.UpdateAsync(appuser);

            if(!Result.Succeeded) 
                return BadRequest(new ApiResponse(400));
            var ReturnedAddress = _mapper.Map<Address, AddressDto>(Mappedaddress);
            return Ok(ReturnedAddress);
        }

        [Authorize]
        [HttpGet("ExistsEmail")]
        public async Task<ActionResult<bool>> CheckEmailExist(string Email)
        => await _userManager.FindByEmailAsync(Email) is not null;

        [HttpPost("ForgetPassword")]
        public async Task<ActionResult> ForgetPassword([FromBody] ForgotPasswordRequest request)
        {
            if (string.IsNullOrEmpty(request.Email))
                return BadRequest(new { message = "Email is required." });

            var user = await _userManager.FindByEmailAsync(request.Email);

            if (user == null)
                return NotFound(new { message = "User not found." });

            var token = await _userManager.GeneratePasswordResetTokenAsync(user);

            var encodedToken = HttpUtility.UrlEncode(token);
            var resetUrl = $"{Request.Scheme}://{Request.Host}/api/Appuser/ResetPassword?token={encodedToken}&email={request.Email}";

            var emailMessage = new EmailMessage
            {
                To = request.Email,
                Subject = "Password Reset Request",
                Body = $"<p>Click <a href='{resetUrl}'>here</a> to reset your password.</p>"
            };

            try
            {
                await _emailService.SendPasswordResetEmailAsync(emailMessage, token, resetUrl);
                return Ok(new { message = "Password reset email sent successfully." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"Failed to send email: {ex.Message}" });
            }
        }

        [HttpGet("ResetPassword")]
        public async Task<IActionResult> ResetPassword(string token, string email)
        {
            if (string.IsNullOrEmpty(token) || string.IsNullOrEmpty(email))
            {
                return BadRequest(new { message = "Token and email are required." });
            }

            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                return NotFound(new { message = "User not found." });
            }

            var isTokenValid = await _userManager.VerifyUserTokenAsync(user, TokenOptions.DefaultProvider, "ResetPassword", token);
            if (!isTokenValid)
            {
                return BadRequest(new { message = "Invalid or expired token." });
            }

            return Ok(new { message = "Token is valid. Proceed to reset password." });
        }
     
        [HttpPost("ResetPassword")]
        public async Task<ActionResult> ResetPassword([FromBody] ResetPasswordRequest request)
        {
            var token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "").Trim();
            var decodedToken = HttpUtility.UrlDecode(token);
          
            if (string.IsNullOrEmpty(token))
            {
                return Unauthorized(new { message = "Token is missing." });
            }

            var email = request.Email;
            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(request.NewPassword))
            {
                return BadRequest(new { message = "Email and new password are required." });
            }

            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                return NotFound(new { message = "User not found." });
            }

            var isTokenValid = await _userManager.VerifyUserTokenAsync(user, TokenOptions.DefaultProvider, "ResetPassword", decodedToken);
            if (!isTokenValid)
            {
                return BadRequest(new { message = "Invalid or expired token." });
            }

            var resetResult = await _userManager.ResetPasswordAsync(user, decodedToken, request.NewPassword);
            if (resetResult.Succeeded)
            {
                return Ok(new ApiResponse(200 , "Password has been reset successfully."));
            }
            return BadRequest(new ApiResponse(400, "Failed to reset password."));

        }

    }
}
