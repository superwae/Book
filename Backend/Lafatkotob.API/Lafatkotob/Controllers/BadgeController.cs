using Lafatkotob.Services.BadgeService;
using Lafatkotob.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace Lafatkotob.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BadgeController : Controller
    {
      
           private readonly IBadgeService _badgeService;
        public BadgeController(IBadgeService badgeService)
        {
                _badgeService = badgeService;
              }
        [HttpGet("getall")]
        public async Task<IActionResult> GetAllBadges()
        {
            var badges = await _badgeService.GetAll();
            if(badges == null) return BadRequest();
            return Ok(badges);
        }
        [HttpGet("getbyid")]
        public async Task<IActionResult> GetBadgeById(int badgeId)
        {
            var badge = await _badgeService.GetById(badgeId);
            if (badge == null) return BadRequest();
            return Ok(badge);
        }
        [HttpPost("post")]
        public async Task<IActionResult> PostBadge(BadgeModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            await _badgeService.Post(model);
            return Ok();
        }
        [HttpDelete("delete")]
        public async Task<IActionResult> DeleteBadge(int badgeId)
        {
            var badge = await _badgeService.Delete(badgeId);
            if (badge == null) return BadRequest();
            return Ok(badge);
        }
        [HttpPut("update")]
        public async Task<IActionResult> UpdateBadge(BadgeModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            await _badgeService.Update(model);
            return Ok();
        }



           
        
    }
}
