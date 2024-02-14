using Lafatkotob.Services.BadgeService;
using Lafatkotob.Services.EventService;
using Lafatkotob.ViewModels;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.Design;

namespace Lafatkotob.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EventController : Controller
    {
        private readonly IEventService _EventService;
        public EventController(IEventService EventService)
        {
            _EventService = EventService;
        }
        [HttpGet("getall")]
        public async Task<IActionResult> GetAllEvent()
        {
            var badges = await _EventService.GetAll();
            if (badges == null) return BadRequest();
            return Ok(badges);
        }
        [HttpGet("getbyid")]
        public async Task<IActionResult> GetEventById(int EventId)
        {
            var Event = await _EventService.GetById(EventId);
            if (Event == null) return BadRequest();
            return Ok(Event);
        }
        [HttpPost("post")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Premium")]

        public async Task<IActionResult> PostEvent(EventModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            await _EventService.Post(model);
            return Ok();
        }
        [HttpDelete("delete")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Premium")]

        public async Task<IActionResult> DeleteEvent(int EventId)
        {
            var Event = await _EventService.Delete(EventId);
            if (Event == null) return BadRequest();
            return Ok(Event);
        }
        [HttpPut("update")]

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Premium")]

        public async Task<IActionResult> UpdateEvent(EventModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            await _EventService.Update(model);
            return Ok();
        }

    }
}
