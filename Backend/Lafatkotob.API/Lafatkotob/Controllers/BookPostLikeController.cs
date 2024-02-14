using Lafatkotob.Services.BookPostLikeServices;
using Lafatkotob.ViewModels;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Lafatkotob.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BookPostLikeController : Controller
    {
        private readonly IBookPostLikeService _bookPostLikeService;
        public BookPostLikeController(IBookPostLikeService bookPostLikeService)
        {
            _bookPostLikeService = bookPostLikeService;
        }
        [HttpGet("getall")]
        public async Task<IActionResult> GetAllBookPostLikes()
        {
            var bookPostLikes = await _bookPostLikeService.GetAll();
            if(bookPostLikes == null) return BadRequest();
            return Ok(bookPostLikes);
        }
        [HttpGet("getbyid")]
        public async Task<IActionResult> GetBookPostLikeById(int bookPostLikeId)
        {
            var bookPostLike = await _bookPostLikeService.GetById(bookPostLikeId);
            if (bookPostLike == null) return BadRequest();
            return Ok(bookPostLike);
        }
        [HttpPost("post")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]

        public async Task<IActionResult> PostBookPostLike(BookPostLikeModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            await _bookPostLikeService.Post(model);
            return Ok();
        }
        [HttpDelete("delete")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]

        public async Task<IActionResult> DeleteBookPostLike(int bookPostLikeId)
        {
            var bookPostLike = await _bookPostLikeService.Delete(bookPostLikeId);
            if (bookPostLike == null) return BadRequest();
            return Ok(bookPostLike);
        }
        [HttpPut("update")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]

        public async Task<IActionResult> UpdateBookPostLike(BookPostLikeModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            await _bookPostLikeService.Update(model);
            return Ok();
        }
        

        
    }
}
