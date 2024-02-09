using Lafatkotob.Entities;
using Lafatkotob.ViewModels;
using Microsoft.EntityFrameworkCore;
using System.Net.NetworkInformation;

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
        public async Task<BooksModel> Post(BooksModel model)
        {
            using (var transaction = await _context.Database.BeginTransactionAsync())
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
                HistoryId= model.HistoryId,
                PublicationDate = model.PublicationDate,
                ISBN = model.ISBN,
                PageCount = model.PageCount,
                Condition = model.Condition,
                Status = model.Status,
                PartnerUserId = model.PartnerUserId
            };
            _context.Books.Add(book);
            await _context.SaveChangesAsync();
            await transaction.CommitAsync();
            model.Id = book.Id;
            return model;
                }   
                catch (Exception)
                {
                    await transaction.RollbackAsync();
                    throw;
                }
            }
        }
        //done getById
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
                Status = Book.Status,
                PartnerUserId = Book.PartnerUserId
            };
         
        }
        //done getAll
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
        PartnerUserId = up.PartnerUserId
       })
       .ToListAsync();   
        }

        //done update
        public async Task<BooksModel> Update(BooksModel model)
        {
            if (model == null) throw new ArgumentNullException(nameof(model));

            var Books = await _context.Books.FindAsync(model.Id);
            if (Books == null) return null;

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
        }
        //done delete
        public async Task<BooksModel> Delete(int id)
        {
            var book = await _context.Books.FindAsync(id);
            if (book == null)
            {
                return null;
            }
            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    _context.Books.Remove(book);
                    await _context.SaveChangesAsync();
                    await transaction.CommitAsync();
                    return new BooksModel
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
                        PartnerUserId = book.PartnerUserId
                    };
                }
                catch (Exception)
                {
                    await transaction.RollbackAsync();
                    throw;
                }

            }
        }
    }
}
