using Azure;
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
        public async Task<ServiceResponse<BookPostLikeModel>> Post(BookPostLikeModel model)
        
        {
            var response = new ServiceResponse<BookPostLikeModel>();
            var executionStrategy = _context.Database.CreateExecutionStrategy();
            await executionStrategy.ExecuteAsync(async () =>
            {
                using (var transaction =  _context.Database.BeginTransaction())
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
                     transaction.Commit();
                        response.Success = true;
                        response.Data = model;
                       
                }
                catch (Exception)
                {
                        transaction.Rollback();
                        response.Success = false;
                        response.Message = "Failed to create badge.";
                }
              }
        });

            return response;
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

        public async Task<ServiceResponse<BookPostLikeModel>> Update(BookPostLikeModel model)
        {
            var response = new ServiceResponse<BookPostLikeModel>();
            if (model == null)
            {
                response.Success = false;
                response.Message = "Model cannot be null.";
                return response;
            }

            var BookPostLike = await _context.BookPostLikes.FindAsync(model.Id);
            if (BookPostLike == null)
            {
                response.Success = false;
                response.Message = "BookPostLike not found.";
                return response;
            }
            var executionStrategy = _context.Database.CreateExecutionStrategy();
            await executionStrategy.ExecuteAsync(async () =>
            {
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

                        response.Success = true;
                        response.Data = model;

                    }
                    catch (Exception ex)
                    {
                        await transaction.RollbackAsync();
                        response.Success = false;
                        response.Message = $"Failed to update badge: {ex.Message}";
                    }
                }

            });
            return response;
        }

        public async Task<ServiceResponse<BookPostLikeModel>> Delete(int id)
        {
            var response = new ServiceResponse<BookPostLikeModel>();
            var BookPostLike = await _context.BookPostLikes.FindAsync(id);
            if (BookPostLike == null)
            {
                response.Success = false;
                response.Message = "Badge not found.";
                return response;
            }
            var executionStrategy = _context.Database.CreateExecutionStrategy();
            await executionStrategy.ExecuteAsync(async () =>
            {
                using (var transaction = await _context.Database.BeginTransactionAsync())
                {
                    try
                    {

                        _context.BookPostLikes.Remove(BookPostLike);
                        await _context.SaveChangesAsync();
                        await transaction.CommitAsync();

                        response.Success = true;
                        response.Data = new BookPostLikeModel
                        {
                            Id = BookPostLike.Id,
                            BookId = BookPostLike.BookId,
                            UserId = BookPostLike.UserId,
                            DateLiked = BookPostLike.DateLiked
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
