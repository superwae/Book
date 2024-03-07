using Lafatkotob.Services.BookService;
using Lafatkotob.ViewModels;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;

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


        [HttpGet("{bookId}")]
        public async Task<IActionResult> GetBookById(int bookId)
        {
            var book = await _bookService.GetById(bookId);
            if (book == null) return NotFound();
            return Ok(book);
        }


        [HttpPost("post")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> PostBook([FromForm] RegisterBook model, IFormFile imageFile)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            if (imageFile != null && !imageFile.ContentType.StartsWith("image/"))
            {
                return BadRequest("Only image files are allowed.");
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
            if (imageFile != null && !imageFile.ContentType.StartsWith("image/"))
            {
                return BadRequest("Only image files are allowed.");
            }
            var result = await _bookService.Update(bookId, model, imageFile);
            if (!result.Success)
            {
                return BadRequest(result.Message);
            }
            return Ok(result.Data);
        }


        [HttpGet("filter")]
        public async Task<IActionResult> GetBooksByGenres([FromQuery] List<int> genreIds)
        {
            if (genreIds == null || !genreIds.Any())
            {
                return BadRequest("No genre IDs provided.");
            }
                
            var books = await _bookService.GetBooksFilteredByGenres(genreIds);
            if (books == null) return BadRequest(books.Message);

            return Ok(books);
        }
        [HttpGet("{bookId}/genres")]
        public async Task<IActionResult> GetGenresForBook(int bookId)
        {
            if (bookId <= 0)
            {
                return BadRequest("Invalid book ID.");
            }

            var genres = await _bookService.GetGenresByBookId(bookId);

            if (genres.Data == null || !genres.Data.Any())
            {
                return NotFound(genres.Message);
            }

            return Ok(genres);
        }
        [HttpGet("search")]
        public async Task<IActionResult> SearchBooks([FromQuery] string query)
        {
            if (string.IsNullOrWhiteSpace(query))
            {
                return BadRequest("Search query is required.");
            }

            var books = await _bookService.SearchBooks(query);
            if (books.Data == null || !books.Data.Any())
            {
                return NotFound(books.Message);
            }

            return Ok(books.Data);
        }
        




    }
}
