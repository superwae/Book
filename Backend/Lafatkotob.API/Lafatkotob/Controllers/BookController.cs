using Lafatkotob.Services.BookService;
using Lafatkotob.ViewModels;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Microsoft.Extensions.Hosting;

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
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> PostBook([FromForm] BooksModel model, IFormFile imageFile)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            var result = await _bookService.Post(model, imageFile);
            if (!result.Success)
            {
                return BadRequest(result.Message);
            }
            return Ok(result.Data);
        }


        [HttpDelete("delete")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]

        public async Task<IActionResult> DeleteBook(int bookId)
        {
           
            var book = await _bookService.Delete(bookId);
                if (book == null) return BadRequest();
            return Ok(book);
        }

        [HttpPut("update/{bookId}"), DisableRequestSizeLimit]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [Consumes("multipart/form-data")] 
        public async Task<IActionResult> UpdateBook(int bookId, [FromForm] UpdateBookModel model,  IFormFile imageFile)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            var result = await _bookService.Update(bookId, model, imageFile);
            if (!result.Success)
            {
                return BadRequest(result.Message);
            }
            return Ok(result.Data);
        }



    }
}
