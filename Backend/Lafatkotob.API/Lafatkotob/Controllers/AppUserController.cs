using Lafatkotob.Services.AppUserService;
using Lafatkotob.ViewModel;
using Lafatkotob.ViewModels;
using login.ViewModel;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Lafatkotob.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AppUserController : ControllerBase
    {
        private readonly IUserService _userService;

        public AppUserController(IUserService userService)
        {
            _userService = userService;
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

        [HttpGet("ConfirmEmail")]
        public async Task<IActionResult> ConfirmEmail(string userId, string token)
        {
            var result = await _userService.ConfirmEmail(userId, token);
            if (!result.Success)
            {
                return BadRequest(result.Message);
            }

            return Ok("Email confirmed successfully.");
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
            var result = await _userService.DeleteUser(userId);
            if (!result.Success)
            {
                return BadRequest(result.Message);
            }

            return Ok("User deleted successfully.");
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpPost("UpdateUser")]
        public async Task<IActionResult> UpdateUser(UpdateUserModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var result = await _userService.UpdateUser(model, userId);
            if (!result.Success)
            {
                return BadRequest(result.Message);
            }

            return Ok("User updated successfully.");
        }

        [HttpPost("Register")]
        public async Task<IActionResult> Register(RegisterModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Assume baseUrl is correctly determined here. For example:
            string baseUrl = $"{Request.Scheme}://{Request.Host}";
            var result = await _userService.RegisterUser(model, baseUrl);
            if (!result.Success)
            {
                return BadRequest(result.Message);
            }

            return Ok(result.Data); // Or customize the response as needed
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
    }
}
