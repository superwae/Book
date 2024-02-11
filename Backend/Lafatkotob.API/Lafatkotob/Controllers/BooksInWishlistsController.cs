using Lafatkotob.Services.BooksInWishlistsService;
using Lafatkotob.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace Lafatkotob.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BooksInWishlistsController : Controller
    {
        private readonly IBooksInWishlistsService _booksInWishlistsService;
        public BooksInWishlistsController(IBooksInWishlistsService booksInWishlistsService)
        {
            _booksInWishlistsService = booksInWishlistsService;
        }
        [HttpGet("getall")]
        public async Task<IActionResult> GetAllBooksInWishlists()
        {
            var booksInWishlists = await _booksInWishlistsService.GetAll();
            if(booksInWishlists == null) return BadRequest();
            return Ok(booksInWishlists);
        }
        [HttpGet("getbyid")]
        public async Task<IActionResult> GetBooksInWishlistsById(int booksInWishlistsId)
        {
            var booksInWishlists = await _booksInWishlistsService.GetById(booksInWishlistsId);
            if (booksInWishlists == null) return BadRequest();
            return Ok(booksInWishlists);
        }
        [HttpPost("post")]
        public async Task<IActionResult> PostBooksInWishlists(BookInWishlistsModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            await _booksInWishlistsService.Post(model);
            return Ok();
        }
        [HttpDelete("delete")]
        public async Task<IActionResult> DeleteBooksInWishlists(int booksInWishlistsId)
        {
            var booksInWishlists = await _booksInWishlistsService.Delete(booksInWishlistsId);
            if (booksInWishlists == null) return BadRequest();
            return Ok(booksInWishlists);
        }
        [HttpPut("update")]
        public async Task<IActionResult> UpdateBooksInWishlists(BookInWishlistsModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            await _booksInWishlistsService.Update(model);
            return Ok();
        }
      
    }
}
