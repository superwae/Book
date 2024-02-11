using Lafatkotob.Services.NotificationService;
using Lafatkotob.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace Lafatkotob.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class NotificationController : Controller
    {
        private readonly INotificationService _notificationService;
        public NotificationController(INotificationService notificationService)
        {
            _notificationService = notificationService;
        }
        [HttpGet("getall")]
        public async Task<IActionResult> GetAllNotifications()
        {
            var notifications = await _notificationService.GetAll();
            if(notifications == null) return BadRequest();
            return Ok(notifications);
        }
        [HttpGet("getbyid")]
        public async Task<IActionResult> GetNotificationById(int notificationId)
        {
            var notification = await _notificationService.GetById(notificationId);
            if (notification == null) return BadRequest();
            return Ok(notification);
        }
        [HttpPost("post")]
        public async Task<IActionResult> PostNotification(NotificationModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            await _notificationService.Post(model);
            return Ok();
        }
        [HttpDelete("delete")]
        public async Task<IActionResult> DeleteNotification(int notificationId)
        {
            var notification = await _notificationService.Delete(notificationId);
            if (notification == null) return BadRequest();
            return Ok(notification);
        }
        [HttpPut("update")]
        public async Task<IActionResult> UpdateNotification(NotificationModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            await _notificationService.Update(model);
            return Ok();
        }
       
    }
}
