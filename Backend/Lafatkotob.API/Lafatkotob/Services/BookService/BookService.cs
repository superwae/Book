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
                        // Handle image saving here and get the path or URL
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
                            CoverImage = imagePath
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
                            PartnerUserId = book.PartnerUserId
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
                PartnerUserId = Book.PartnerUserId
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
              PartnerUserId = up.PartnerUserId
          })
              .ToListAsync();
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
                        // For CoverImage, handle separately as it involves file processing
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
                            PartnerUserId = book.PartnerUserId
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
    }
}
