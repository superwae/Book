using Lafatkotob.Services.WishListService;
using Lafatkotob.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace Lafatkotob.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class WishlistController : Controller
    {
        private readonly IWishListService _wishlistService;
        public WishlistController(IWishListService wishlistService)
        {
            _wishlistService = wishlistService;
        }
        [HttpGet("getall")]
        public async Task<IActionResult> GetAllWishlists()
        {
            var wishlists = await _wishlistService.GetAll();
            if(wishlists == null) return BadRequest();
            return Ok(wishlists);
        }
        [HttpGet("getbyid")]
        public async Task<IActionResult> GetWishlistById(int wishlistId)
        {
            var wishlist = await _wishlistService.GetById(wishlistId);
            if (wishlist == null) return BadRequest();
            return Ok(wishlist);
        }
        [HttpPost("post")]
        public async Task<IActionResult> PostWishlist(WishlistModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            await _wishlistService.Post(model);
            return Ok();
        }
        [HttpDelete("delete")]
        public async Task<IActionResult> DeleteWishlist(int wishlistId)
        {
            var wishlist = await _wishlistService.Delete(wishlistId);
            if (wishlist == null) return BadRequest();
            return Ok(wishlist);
        }
        [HttpPut("update")]
        public async Task<IActionResult> UpdateWishlist(WishlistModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            await _wishlistService.Update(model);
            return Ok();
        }
        
    }
}
