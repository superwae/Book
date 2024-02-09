using Lafatkotob.Entities;
using Lafatkotob.ViewModels;
using Microsoft.EntityFrameworkCore;
using System.Net;
using static System.Reflection.Metadata.BlobBuilder;

namespace Lafatkotob.Services.BookGenreService
{
    public class BookGenreServices : IBookGenreServices
    {
        private readonly ApplicationDbContext _context;
        public BookGenreServices(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<BookGenreModel> Post(BookGenreModel model)
        {
            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    var BookGenre = new BookGenre
                    {
                        Id = model.Id,
                        BookId= model.BookId,
                        GenreId = model.GenreId
                    };

                    _context.BookGenres.Add(BookGenre);
                    await _context.SaveChangesAsync();
                    await transaction.CommitAsync();

                    model.Id = BookGenre.Id;
                    return model;
                }
                catch (Exception)
                {
                    await transaction.RollbackAsync();
                    throw;
                }
            }
        }
        public async Task<BookGenreModel> GetById(int id)
        {
            var BookGenre = await _context.BookGenres.FindAsync(id);
            if (BookGenre == null) return null;

            return new BookGenreModel
            {
                Id = BookGenre.Id,
                BookId = BookGenre.BookId,
                GenreId = BookGenre.GenreId
            };
        }

        public async Task<List<BookGenreModel>> GetAll()
        {
            return await _context.BookGenres
                .Select(up => new BookGenreModel
                {
                    Id = up.Id,
                    BookId = up.BookId,
                    GenreId = up.GenreId
                })
                .ToListAsync();
        }
        public async Task<BookGenreModel> Update(BookGenreModel model)
        {
            if(model == null) throw new ArgumentNullException(nameof(model));
            var BookGenre = await _context.BookGenres.FindAsync(model.Id);
            if (BookGenre == null) return null;
            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                { 
                    BookGenre.Id = model.Id;
                    BookGenre.BookId = model.BookId;
                    BookGenre.GenreId = model.GenreId;
                    _context.BookGenres.Update(BookGenre);
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
        public async Task<BookGenreModel> Delete(int id)
        {
            var BookGenre = await _context.BookGenres.FindAsync(id);
            if (BookGenre == null) return null;
            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    _context.BookGenres.Remove(BookGenre);
                    await _context.SaveChangesAsync();
                    await transaction.CommitAsync();
                    return new BookGenreModel
                    {
                        Id = BookGenre.Id,
                        BookId = BookGenre.BookId,
                        GenreId = BookGenre.GenreId
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
