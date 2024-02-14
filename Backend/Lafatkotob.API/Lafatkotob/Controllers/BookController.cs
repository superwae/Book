using Lafatkotob.Services.BookService;
using Lafatkotob.ViewModels;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Lafatkotob.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BookController : Controller
    {
        private readonly IBookService _bookService;
        public BookController(IBookService bookService)
        {
            _bookService = bookService;
        }
        [HttpGet("getall")]
        public async Task<IActionResult> GetAllBooks()
        {
            var books = await _bookService.GetAll();
            if(books == null) return BadRequest();
            return Ok(books);
        }
        [HttpGet("getbyid")]
        public async Task<IActionResult> GetBookById(int bookId)
        {
            var book = await _bookService.GetById(bookId);
            if (book == null) return BadRequest();
            return Ok(book);
        }
        [HttpPost("post")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> PostBook(BooksModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            await _bookService.Post(model);
            return Ok();
        }
        [HttpDelete("delete")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]

        public async Task<IActionResult> DeleteBook(int bookId)
        {

            var book = await _bookService.Delete(bookId);
                if (book == null) return BadRequest();
            return Ok(book);
        }
        [HttpPut("update")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]

        public async Task<IActionResult> UpdateBook(BooksModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            await _bookService.Update(model);
            return Ok();
        }
       
    }
}
