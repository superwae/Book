﻿using Lafatkotob.Entities;
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

        [HttpGet("user/{userId}/events")]
        public async Task<ActionResult<List<EventModel>>> GetUserEvents(string userId)
        {
            var events = await _EventService.GetEventsByUserId(userId);

            if (events == null || !events.Any())
            {
                return NotFound("No events found for the given user.");
            }

            return Ok(events);
        }

        [HttpPost("post")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Premium,Admin")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> PostEvent([FromForm] EventModel model, IFormFile imageFile)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            if (imageFile != null && !imageFile.ContentType.StartsWith("image/"))
            {
                return BadRequest("Only image files are allowed.");
            }
            var result = await _EventService.Post(model, imageFile);
            if (!result.Success)
            {
                return BadRequest(result.Message);
            }
            return Ok(result.Data);
        }


        [HttpDelete("delete")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Premium,Admin")]

        public async Task<IActionResult> DeleteEvent(int EventId)
        {
            var Event = await _EventService.Delete(EventId);
            if (Event == null) return BadRequest();
            return Ok(Event);
        }
        [HttpPut("update/{eventId}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Premium,Admin")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> UpdateEvent(int eventId, [FromForm] EventModel model, IFormFile imageFile)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            if (imageFile != null && !imageFile.ContentType.StartsWith("image/"))
            {
                return BadRequest("Only image files are allowed.");
            }
            var result = await _EventService.Update(eventId, model, imageFile);
            if (!result.Success)
            {
                return BadRequest(result.Message);
            }
            return Ok(result.Data);
        }

    }
}
