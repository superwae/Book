﻿using Lafatkotob.Services.MessageService;
using Lafatkotob.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace Lafatkotob.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MessageController : Controller
    {
        private readonly IMessageService _messageService;
        public MessageController(IMessageService messageService)
        {
            _messageService = messageService;
        }
        [HttpGet("getall")]
        public async Task<IActionResult> GetAllMessages()
        {
            var messages = await _messageService.GetAll();
            if(messages == null) return BadRequest();
            return Ok(messages);
        }
        [HttpGet("getbyid")]
        public async Task<IActionResult> GetMessageById(int messageId)
        {
            var message = await _messageService.GetById(messageId);
            if (message == null) return BadRequest();
            return Ok(message);
        }
        [HttpPost("post")]
        public async Task<IActionResult> PostMessage(MessageModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            await _messageService.Post(model);
            return Ok();
        }
        [HttpDelete("delete")]
        public async Task<IActionResult> DeleteMessage(int messageId)
        {
            var message = await _messageService.Delete(messageId);
            if (message == null) return BadRequest();
            return Ok(message);
        }
        [HttpPut("update")]
        public async Task<IActionResult> UpdateMessage(MessageModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            await _messageService.Update(model);
            return Ok();
        }
       
    }
}
