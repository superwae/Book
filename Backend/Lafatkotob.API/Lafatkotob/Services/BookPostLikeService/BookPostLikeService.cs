using Lafatkotob.Entities;
using Lafatkotob.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace Lafatkotob.Services.BookPostLikeServices
{
    public class BookPostLikeService : IBookPostLikeService
    {
        private readonly ApplicationDbContext _context;
        public BookPostLikeService(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<BookPostLikeModel> Post(BookPostLikeModel model)
        {
            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    var bookPostLike = new BookPostLike
                    {
                        Id = model.Id,
                        BookId = model.BookId,
                        UserId = model.UserId,
                        DateLiked = DateTime.Now
                    };
                    _context.BookPostLikes.Add(bookPostLike);
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
            public async Task<BookPostLikeModel> GetById(int id)
            {
                var bookPostLike = await _context.BookPostLikes.FindAsync(id);
                if (bookPostLike == null) return null;
                return new BookPostLikeModel
                {
                    Id = bookPostLike.Id,
                    BookId = bookPostLike.BookId,
                    UserId = bookPostLike.UserId,
                    DateLiked = bookPostLike.DateLiked
                };
            }
            public async Task<List<BookPostLikeModel>> GetAll()
            {
                return await _context.BookPostLikes.Select(bookPostLike => new BookPostLikeModel
                {
                    Id = bookPostLike.Id,
                    BookId = bookPostLike.BookId,
                    UserId = bookPostLike.UserId,
                    DateLiked = bookPostLike.DateLiked
                }).ToListAsync();
            }

            public async Task<BookPostLikeModel> Update(BookPostLikeModel model)
            {
                if (model == null) throw new ArgumentNullException(nameof(model));

                var BookPostLike = await _context.BookPostLikes.FindAsync(model.Id);
                if (BookPostLike == null) return null;
                using (var transaction = await _context.Database.BeginTransactionAsync())
                {
                    try
                    {
                        BookPostLike.Id = model.Id;
                        BookPostLike.BookId = model.BookId;
                        BookPostLike.UserId = model.UserId;
                        BookPostLike.DateLiked = model.DateLiked;

                        _context.BookPostLikes.Update(BookPostLike);
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

            public async Task<BookPostLikeModel> Delete(int id)
            {
                var BookPostLike = await _context.BookPostLikes.FindAsync(id);
                if (BookPostLike == null)
                {
                    return null;
                }
                using (var transaction = await _context.Database.BeginTransactionAsync())
                {
                    try
                    {
                        var bookPostLike = await _context.BookPostLikes.FindAsync(id);
                        _context.BookPostLikes.Remove(bookPostLike);
                        await _context.SaveChangesAsync();
                        return new BookPostLikeModel
                        {
                            Id = bookPostLike.Id,
                            BookId = bookPostLike.BookId,
                            UserId = bookPostLike.UserId,
                            DateLiked = bookPostLike.DateLiked
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
