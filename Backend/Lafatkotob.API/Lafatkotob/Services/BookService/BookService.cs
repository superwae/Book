using Lafatkotob.Entities;
using Lafatkotob.ViewModels;
using Microsoft.EntityFrameworkCore;
using System.Net.NetworkInformation;
using System.Security.Claims;

namespace Lafatkotob.Services.BookService
{
    public class BookService: IBookService
    {
private readonly ApplicationDbContext _context;
        public BookService(ApplicationDbContext context)
        {
            _context = context;
        }
        //done post
        public async Task<ServiceResponse<BooksModel>> Post(BooksModel model)
        {
            var response = new ServiceResponse<BooksModel>();

            var executionStrategy = _context.Database.CreateExecutionStrategy();
            await executionStrategy.ExecuteAsync(async () =>
            {
                using (var transaction = _context.Database.BeginTransaction())
                {
                    try
                    {
                        var book = new Book
                        {
                            Id = model.Id,
                            Title = model.Title,
                            Author = model.Author,
                            Description = model.Description,
                            CoverImage = model.CoverImage,
                            UserId = model.UserId,
                            HistoryId = model.HistoryId,
                            PublicationDate = model.PublicationDate,
                            ISBN = model.ISBN,
                            PageCount = model.PageCount,
                            Condition = model.Condition,
                            Type = model.Type,
                            Status = model.Status,
                            PartnerUserId = model.PartnerUserId

                        };
                        _context.Books.Add(book);
                        await _context.SaveChangesAsync();
                        transaction.Commit();
                        response.Success = true;
                        response.Data = model;
                    }
                    catch (Exception )
                    {
                        transaction.Rollback();
                        response.Success = false;
                        response.Message = "Failed to create badge.";
                    }
                }
            });
            return response;
        }

        public async Task<BooksModel> GetById(int id)
        {
            var Book = await _context.Books.FindAsync(id);
            if (Book == null) return null;

            return new BooksModel
            {
                Id = Book.Id,
                Title = Book.Title,
                Author = Book.Author,
                Description = Book.Description,
                CoverImage = Book.CoverImage,
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
       
        public async Task<List<BooksModel>> GetAll()
        {

         return await _context.Books   
       .Select(up => new BooksModel
       {
        Id = up.Id,
        Title = up.Title,
        Author = up.Author,
        Description = up.Description,
        CoverImage = up.CoverImage,
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

       
        public async Task<ServiceResponse<BooksModel>> Update(BooksModel model)
        {
            var response = new ServiceResponse<BooksModel>();
            if (model == null) {
                response.Success = false;
                response.Message = "Model cannot be null.";
                return response;
            }

            var Books = await _context.Books.FindAsync(model.Id);
            if (Books == null)
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
                        Books.Id = model.Id;
                        Books.Title = model.Title;
                        Books.Author = model.Author;
                        Books.Description = model.Description;
                        Books.CoverImage = model.CoverImage;
                        Books.UserId = model.UserId;
                        Books.HistoryId = model.HistoryId;
                        Books.PublicationDate = model.PublicationDate;
                        Books.ISBN = model.ISBN;
                        Books.PageCount = model.PageCount;
                        Books.Condition = model.Condition;
                        Books.Status = model.Status;
                        Books.Type = model.Type;
                        Books.PartnerUserId = model.PartnerUserId;

                        _context.Books.Update(Books);
                        await _context.SaveChangesAsync();
                        await transaction.CommitAsync();

                        return model;
                    }

                    catch (Exception)
                    {
                        await transaction.RollbackAsync();
                        throw;
                    }
                }
            });
            return response;
        }
        //done delete
        public async Task<ServiceResponse<BooksModel>> Delete(int id)
        {
            var response = new ServiceResponse<BooksModel>();
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
                        response.Data = new BooksModel
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
    }
}
