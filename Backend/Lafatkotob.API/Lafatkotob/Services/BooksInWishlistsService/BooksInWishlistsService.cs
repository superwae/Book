using Lafatkotob.Entities;
using Lafatkotob.ViewModels;
using Microsoft.EntityFrameworkCore;
using static System.Reflection.Metadata.BlobBuilder;

namespace Lafatkotob.Services.BooksInWishlistsService
{
    public class BooksInWishlistsService : IBooksInWishlistsService
    {
private readonly ApplicationDbContext _context;
        public BooksInWishlistsService(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<BookInWishlistsModel> Delete(int id)
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
                    return new BookInWishlistsModel
                    {
                        Id = book.Id,
                        Title = book.Title,
                        Author = book.Author,
                        ISBN = book.ISBN,

                    };
                }
                catch (Exception)
                {
                    await transaction.RollbackAsync();
                    throw;
                }
            }
        }
        public async Task<List<BookInWishlistsModel>> GetAll()
        {
            return await _context.BooksInWishlists.Select(book => new BookInWishlistsModel
            {
                Id = book.Id,
                Title = book.Title,
                Author = book.Author,
                ISBN = book.ISBN,
            }).ToListAsync();
        }
        public async Task<BookInWishlistsModel> GetById(int id)
        {
            var BookInWishList = await _context.BooksInWishlists.FindAsync(id);
            if (BookInWishList == null) return null;
            return new BookInWishlistsModel
            {
                Id = BookInWishList.Id,
                Title = BookInWishList.Title,
                Author = BookInWishList.Author,
                ISBN = BookInWishList.ISBN
            };

        }
        public async Task<BookInWishlistsModel> Post(BookInWishlistsModel model)
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
                        ISBN = model.ISBN,
                       
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
        public async Task<BookInWishlistsModel> Update(BookInWishlistsModel model)
        {
            if (model == null) throw new ArgumentNullException(nameof(model));

            var BookInWischlist = await _context.BooksInWishlists.FindAsync(model.Id);
            if (BookInWischlist == null) return null;

            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    BookInWischlist.Id= model.Id;
                    BookInWischlist.Title = model.Title;
                    BookInWischlist.Author = model.Author;
                    BookInWischlist.ISBN = model.ISBN;
                    _context.BooksInWishlists.Update(BookInWischlist);
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

    }
}
