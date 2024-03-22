using Lafatkotob.Services.ConversationService;
using Lafatkotob.ViewModels;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Lafatkotob.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ConversationController : Controller
    {
       
        private readonly IConversationService _conversationService;
        public ConversationController(IConversationService conversationService)
        {
                _conversationService = conversationService;
        }

        [HttpGet("getall")]
        public async Task<IActionResult> GetAllConversations()
        {
            var conversations = await _conversationService.GetAll();
            if(conversations == null) return BadRequest();
            return Ok(conversations);
        }

        [HttpGet("getbyid")]
        public async Task<IActionResult> GetConversationById(int conversationId)
        {
            var conversation = await _conversationService.GetById(conversationId);
            if (conversation == null) return BadRequest();
            return Ok(conversation);
        }

        [HttpPost("post")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]

        public async Task<IActionResult> PostConversation(ConversationModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            await _conversationService.Post(model);
            return Ok();
        }

        [HttpDelete("delete")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]

        public async Task<IActionResult> DeleteConversation(int conversationId)
        {
            var conversation = await _conversationService.Delete(conversationId);
            if (conversation == null) return BadRequest();
            return Ok(conversation);
        }

        [HttpPut("update")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> UpdateConversation(ConversationModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            await _conversationService.Update(model);
            return Ok();
        }
    }
}
