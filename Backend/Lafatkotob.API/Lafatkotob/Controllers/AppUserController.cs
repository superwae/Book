using Lafatkotob.Entities;
using Lafatkotob.Services.AppUserService;
using Lafatkotob.Services.EmailService;
using Lafatkotob.Services.TokenService;
using Lafatkotob.ViewModel;
using login.ViewModel;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Lafatkotob.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AppUserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IEmailService _emailService;
        private readonly ITokenSerive _tokenService;
        private readonly UserManager<AppUser> _userManager;
        public AppUserController(IUserService userService, IEmailService emailService
            , ITokenSerive tokenService, UserManager<AppUser> userManager)
        {
            _userService = userService;
            _emailService = emailService;
            _tokenService = tokenService;
            _userManager = userManager;
        }
       
        [HttpGet("getall")]
        public async Task<IActionResult> GetAllUsers()
        {
            var users = await _userService.GetAllUsers();
            if(User == null) return BadRequest();
            return Ok(users);
        }
        [HttpGet("getbyid")]
        public async Task<IActionResult> GetUserById(string userId)
        {
            var user = await _userService.GetUserById(userId);
            if (user == null) return BadRequest();
            return Ok(user);
        }
        [HttpDelete("delete")]
        public async Task<IActionResult> DeleteUser(string userId)
        {
            var user = await _userService.DeleteUser(userId);
            if (user == null) return BadRequest();
            return Ok(user);
        }
        [HttpPut("update")]
        public async Task<IActionResult> UpdateUser(UpdateUserModel model, string userId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            await _userService.UpdateUser(model, userId);
            return Ok();
        }

      
    }
}
