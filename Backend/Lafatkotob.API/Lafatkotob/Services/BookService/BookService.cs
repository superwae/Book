using Lafatkotob.Entities;
using Lafatkotob.ViewModels;
using Microsoft.EntityFrameworkCore;
using System.Net.NetworkInformation;
using System.Security.Claims;

namespace Lafatkotob.Services.BookService
{
    public class BookService : IBookService
    {
        private readonly ApplicationDbContext _context;
        public BookService(ApplicationDbContext context)
        {
            _context = context;
        }


        public async Task<ServiceResponse<BookModel>> Post(RegisterBook model, IFormFile imageFile)
        {
            var response = new ServiceResponse<BookModel>();
            var executionStrategy = _context.Database.CreateExecutionStrategy();

            await executionStrategy.ExecuteAsync(async () =>
            {
                using (var transaction = await _context.Database.BeginTransactionAsync())
                {
                    try
                    {
                        var imagePath = await SaveImageAsync(imageFile);

                        var book = new Book
                        {
                            Title = model.Title,
                            Author = model.Author,
                            Description = model.Description,
                            UserId = model.UserId,
                            HistoryId = model.HistoryId,
                            PublicationDate = model.PublicationDate,
                            ISBN = model.ISBN,
                            PageCount = model.PageCount,
                            Condition = model.Condition,
                            Type = model.Type,
                            Status = model.Status,
                            PartnerUserId = model.PartnerUserId,
                            CoverImage = imagePath,
                            Language = model.Language,
                            AddedDate = DateTime.Now

                        };
                        

                        _context.Books.Add(book);
                        await _context.SaveChangesAsync();
                        var BookModel = new BookModel
                        {
                            Id= book.Id,
                            Title = book.Title,
                            Author = book.Author,
                            Description = book.Description,
                            CoverImage = book.CoverImage,
                            UserId = book.UserId,
                            HistoryId = book.HistoryId,
                            PublicationDate = book.PublicationDate,
                            ISBN = book.ISBN,
                            PageCount = book.PageCount,
                            Condition = book.Condition,
                            Status = book.Status,
                            Type = book.Type,
                            PartnerUserId = book.PartnerUserId,
                            Language = book.Language,
                            AddedDate = DateTime.Now
                        };
                        await transaction.CommitAsync();

                        BookModel.CoverImage = imagePath; // Update model with image path for response
                        response.Success = true;
                        response.Data = BookModel;
                    }
                    catch (Exception ex)
                    {
                        await transaction.RollbackAsync();
                        response.Success = false;
                        response.Message = $"Failed to create book: {ex.Message}";
                    }
                }
            });

            return response;
        }



        public async Task<BookModel> GetById(int id)
        {
            var Book = await _context.Books.FindAsync(id);
            if (Book == null) return null;

            return new BookModel
            {
                Id = Book.Id,
                Title = Book.Title,
                Author = Book.Author,
                Description = Book.Description,
                CoverImage = ConvertToFullUrl(Book.CoverImage),
                UserId = Book.UserId,
                HistoryId = Book.HistoryId,
                PublicationDate = Book.PublicationDate,
                ISBN = Book.ISBN,
                PageCount = Book.PageCount,
                Condition = Book.Condition,
                Type = Book.Type,
                Status = Book.Status,
                PartnerUserId = Book.PartnerUserId,
                Language = Book.Language,
                AddedDate = DateTime.Now
            };

        }

        public async Task<List<BookModel>> GetAll()
        {

            return await _context.Books
          .Select(up => new BookModel
          {
              Id = up.Id,
              Title = up.Title,
              Author = up.Author,
              Description = up.Description,
              CoverImage = ConvertToFullUrl(up.CoverImage),
              UserId = up.UserId,
              HistoryId = up.HistoryId,
              PublicationDate = up.PublicationDate,
              ISBN = up.ISBN,
              PageCount = up.PageCount,
              Condition = up.Condition,
              Status = up.Status,
              Type = up.Type,
              PartnerUserId = up.PartnerUserId,
              Language = up.Language,
              AddedDate = DateTime.Now
          })
              .ToListAsync();
        }





        public async Task<ServiceResponse<List<Book>>> SearchBooks(string query)
        {
            var response = new ServiceResponse<List<Book>>();
            query = query.ToLower().Trim();

            var books = await _context.Books
                .Where(b => b.Title.ToLower().Contains(query) || b.Author.ToLower().Contains(query))
                .ToListAsync();

            string baseUrl = "https://localhost:7139";

            books.ForEach(b =>
            {
                if (!string.IsNullOrWhiteSpace(b.CoverImage) && !b.CoverImage.StartsWith("http"))
                {
                    b.CoverImage = $"{baseUrl}{(b.CoverImage.StartsWith('/') ? "" : "/")}{b.CoverImage}";
                }
            });

            response.Success = true;
            response.Data = books;
            if (books.Count == 0)
            {
                response.Message = "No books found matching the search criteria.";
                response.Success = false;
                return response;
            }
            return response;
        }


        

        private string FormatCoverImageUrl(string baseUrl, string coverImagePath)
        {
            if (!string.IsNullOrWhiteSpace(coverImagePath) && !coverImagePath.StartsWith("http"))
            {
                return $"{baseUrl}{(coverImagePath.StartsWith('/') ? "" : "/")}{coverImagePath}";
            }
            return coverImagePath;
        }



        public async Task<ServiceResponse<UpdateBookModel>> Update(int id, UpdateBookModel model, IFormFile imageFile = null)
        {
            var response = new ServiceResponse<UpdateBookModel>();
            if (model == null)
            {
                response.Success = false;
                response.Message = "Model cannot be null.";
                return response;
            }

            var book = await _context.Books.FindAsync(id);
            if (book == null)
            {
                response.Success = false;
                response.Message = "Book not found.";
                return response;
            }

            var executionStrategy = _context.Database.CreateExecutionStrategy();
            await executionStrategy.ExecuteAsync(async () =>
            {
                using (var transaction = await _context.Database.BeginTransactionAsync())
                {
                    try
                    {
                        book.Title = model.Title ?? book.Title;
                        book.Author = model.Author ?? book.Author;
                        book.Description = model.Description ?? book.Description;
                        if (imageFile != null)
                        {
                            var imagePath = await SaveImageAsync(imageFile);
                            book.CoverImage = imagePath;
                        }
                        book.ISBN = model.ISBN ?? book.ISBN;
                        book.PageCount = model.PageCount ?? book.PageCount;
                        book.Condition = model.Condition ?? book.Condition;
                        book.Status = model.Status ?? book.Status;
                        book.Type = model.Type ?? book.Type;
                        book.Language = model.Language;
                        book.AddedDate = DateTime.Now;


                        _context.Books.Update(book);
                        await _context.SaveChangesAsync();
                        await transaction.CommitAsync();

                        response.Success = true;
                        response.Data = model; 
                    }
                    catch (Exception ex)
                    {
                        await transaction.RollbackAsync();
                        response.Success = false;
                        response.Message = $"Failed to update book: {ex.Message}";
                    }
                }
            });

            return response;
        }

        private async Task<string> SaveImageAsync(IFormFile imageFile)
        {
            if (imageFile == null || imageFile.Length == 0)
            {
                throw new ArgumentException("The file is empty or null.", nameof(imageFile));
            }

            // Ensure the uploads directory exists
            var uploadsFolderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads");
            if (!Directory.Exists(uploadsFolderPath))
            {
                Directory.CreateDirectory(uploadsFolderPath);
            }

            // Generate a unique filename for the image to avoid name conflicts
            var fileName = Guid.NewGuid().ToString() + Path.GetExtension(imageFile.FileName);
            var filePath = Path.Combine(uploadsFolderPath, fileName);

            // Save the file
            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                await imageFile.CopyToAsync(fileStream);
            }

            var imageUrl = $"/uploads/{fileName}";
            return imageUrl;
        }



        public async Task<ServiceResponse<BookModel>> Delete(int id)
        {
            var response = new ServiceResponse<BookModel>();
            var book = await _context.Books.FindAsync(id);
            if (book == null)
            {
                response.Success = false;
                response.Message = "book not found.";
                return response;
            }

            var executionStrategy = _context.Database.CreateExecutionStrategy();
            await executionStrategy.ExecuteAsync(async () =>
            {
                using (var transaction = await _context.Database.BeginTransactionAsync())
                {
                    try
                    {
                        _context.Books.Remove(book);
                        await _context.SaveChangesAsync();
                        await transaction.CommitAsync();
                        response.Success = true;
                        response.Data = new BookModel
                        {
                            Id = book.Id,
                            Title = book.Title,
                            Author = book.Author,
                            Description = book.Description,
                            CoverImage = book.CoverImage,
                            UserId = book.UserId,
                            HistoryId = book.HistoryId,
                            PublicationDate = book.PublicationDate,
                            ISBN = book.ISBN,
                            PageCount = book.PageCount,
                            Condition = book.Condition,
                            Status = book.Status,
                            Type = book.Type,
                            PartnerUserId = book.PartnerUserId,
                            Language = book.Language,
                            AddedDate = book.AddedDate
                        };
                    }
                    catch (Exception ex)
                    {
                        await transaction.RollbackAsync();
                        response.Success = false;
                        response.Message = $"Failed to delete badge: {ex.Message}";
                    }

                }
            });
            return response;
        }
        private static  string ConvertToFullUrl(string relativePath)
        {
            if (string.IsNullOrEmpty(relativePath))
                return null;

            var baseUrl = "https://localhost:7139"; 
            return $"{baseUrl}{relativePath}";
        }


        public async Task<ServiceResponse<List<BookModel>>> GetBooksFilteredByGenres(List<int> genreIds)
        {
            var response = new ServiceResponse<List<BookModel>>();

            var books =  await _context.Books
                    .Where(b => b.BookGenres.Any(bg => genreIds.Contains(bg.GenreId)))
                    .Select(b => new BookModel
                    {
                        Id = b.Id,
                        Title = b.Title,
                        Author = b.Author,
                        Description = b.Description,
                        CoverImage = ConvertToFullUrl(b.CoverImage),
                        UserId = b.UserId,
                        HistoryId = b.HistoryId,
                        PublicationDate = b.PublicationDate,
                        ISBN = b.ISBN,
                        PageCount = b.PageCount,
                        Condition = b.Condition,
                        Status = b.Status,
                        Type = b.Type,
                        PartnerUserId = b.PartnerUserId,
                        Language = b.Language,
                        AddedDate = b.AddedDate

                    })
                    .ToListAsync();
            response.Data = books;
            response.Success = true;
            if(books.Count == 0)
            {
                response.Message = "No books found for the specified genres.";
                response.Success = false;
                return response;
            }
            return response;
        }

        public async Task<ServiceResponse<List<GenreModel>>> GetGenresByBookId(int bookId)
        {
            var response = new ServiceResponse<List<GenreModel>>();
            var geners= await _context.BookGenres
                   .Where(bg => bg.BookId == bookId)
                   .Select(bg => bg.Genre)
                   .Select(g => new GenreModel
                   {
                       Id = g.Id,
                       Name = g.Name,
                   })
                   .ToListAsync();
            response.Data = geners;
            response.Success = true;
            if (geners.Count == 0)
            {
                response.Message = "No genres found for the specified book.";
                response.Success = false;
                return response;
            }
            return response;

        }
    }
}
