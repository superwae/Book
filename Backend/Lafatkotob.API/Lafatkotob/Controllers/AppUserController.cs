using Lafatkotob.Entities;
using Lafatkotob.Services.AppUserService;
using Lafatkotob.Services.EmailService;
using Lafatkotob.ViewModel;
using Lafatkotob.ViewModels;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Lafatkotob.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AppUserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly UserManager<AppUser> _userManager;
        private readonly IEmailService _emailService;

        public AppUserController(IUserService userService, IEmailService emailService, UserManager<AppUser>userManager)
        {
            _userService = userService;
            _emailService = emailService;
            _userManager = userManager;
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login(LoginModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var loginResult = await _userService.LoginUser(model);
            if (!loginResult.Success)
            {
                return Unauthorized(loginResult.ErrorMessage);
            }

            return Ok(new { Token = loginResult.Token });
        }


        
        [HttpGet("getall")]
        public async Task<IActionResult> GetAllUsers()
        {
            var users = await _userService.GetAllUsers();
            return Ok(users);
        }

        [HttpGet("getbyid")]
        public async Task<IActionResult> GetUserById(string userId)
        {
            var user = await _userService.GetUserById(userId);
            if (user == null) return NotFound("User not found.");
            return Ok(user);
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpDelete("DeleteUser")]
        public async Task<IActionResult> DeleteUser(string userId)
        {
            var UserName = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = await _userManager.FindByIdAsync(userId);
            
            if(user.UserName != UserName && !User.HasClaim(ClaimTypes.Role, "Admin"))
            {
                return BadRequest("you are not authorized to delete this user");
            }
            var result = await _userService.DeleteUser(userId);
            if (!result.Success)
            {
                return BadRequest(result.Message);
            }

            return Ok("User deleted successfully.");
        }


        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpPut("UpdateUser")]
        public async Task<IActionResult> UpdateUser(UpdateUserModel model, string userId)

        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var UserName = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = await _userManager.FindByIdAsync(userId);

            if (user.UserName != UserName && !User.HasClaim(ClaimTypes.Role, "Admin"))
            {
                return BadRequest("you are not authorized to update this user");
            }
            var result = await _userService.UpdateUser(model, userId);
            if (!result.Success)
            {
                return BadRequest(result.Message);
            }

            return Ok("User updated successfully.");
        }

        [HttpPost("Register")]
            public async Task<IActionResult> Register([FromBody] RegisterModel model, [FromQuery] string role)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _userService.RegisterUser(model, role);
            if (!result.Success)
            {
                return BadRequest(result.Message);
            }
            
                var token = await _userManager.GenerateEmailConfirmationTokenAsync(result.Data);
                var confirmationLink = Url.Action("ConfirmEmail", "Account",
                                        new { userId = result.Data.Id, token = token },
                                        protocol: Request.Scheme);

                string subject = "Confirm Your Account";
                string body = $@"Hello {result.Data.UserName},<br><br>
                                Please confirm your account by clicking the link below:<br>
                                <a href='{confirmationLink}'>Confirm Your Account</a><br><br>
                                Thank you.";

                await _emailService.SendEmailAsync(result.Data.Email, subject, body);


            return Ok(result.Data); 
        }

        [HttpGet("validateToken")]
        public IActionResult ValidateToken()
        {
            var isAuthenticated = HttpContext.User.Identity.IsAuthenticated;
            if (!isAuthenticated)
            {
                return Unauthorized("Token is invalid or expired.");
            }
            var userName = HttpContext.User.Identity.Name;
            var userIdClaim = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            return Ok(new { UserName = userName, UserId = userIdClaim });
        }
        [HttpPost]
        [Route("forgot-password")]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var user = await _userManager.Users.FirstOrDefaultAsync(u => u.Email == model.Email);
            //var user = await _userManager.FindByEmailAsync(model.Email); use this when the email in unique
            if (user != null) 
            {
                var token = await _userManager.GeneratePasswordResetTokenAsync(user);
             

                var frontendResetLink = $"http://localhost:4200/reset-password?email={Uri.EscapeDataString(model.Email)}&token={Uri.EscapeDataString(token)}";
                await _emailService.SendPasswordResetEmailAsync(model.Email, frontendResetLink, user.UserName);

            }
            return Ok(new { Message = "Please check your email to reset your password." });
        }
        [HttpPut]
        [Route("reset-password")]
        public async Task<IActionResult> ResetPassword(ResetPasswordModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = await _userManager.Users.FirstOrDefaultAsync(u => u.Email == model.Email);
            //var user = await _userManager.FindByEmailAsync(model.Email); use this when the email in unique
            if (user == null)
            {

                return BadRequest("Invalid request");
            }

            var result = await _userManager.ResetPasswordAsync(user, model.Token, model.NewPassword);
            if (result.Succeeded)
            {
                return Ok(new { Message = "Password has been reset successfully." });
            }
            return BadRequest(result.Errors);
        }


    }
}
