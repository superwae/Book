using Lafatkotob.Entities;
using Lafatkotob.ViewModels;
using Microsoft.EntityFrameworkCore;
using static System.Reflection.Metadata.BlobBuilder;

namespace Lafatkotob.Services.BookPostCommentService
{
    public class BookPostCommentServices : IBookPostCommentServices
    {
        private readonly ApplicationDbContext _context;
        public BookPostCommentServices(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<BookPostCommentModel> Post(BookPostCommentModel model)
        {
            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    var BookPostComment = new BookPostComment
                    {
                        Id = model.Id,
                        CommentText = model.CommentText,
                        UserId = model.UserId,
                        DateCommented = DateTime.Now,
                        BookId = model.BookId,

                    };
                    _context.BookPostComments.Add(BookPostComment);
                    await _context.SaveChangesAsync();
                    await transaction.CommitAsync();
                    model.Id = BookPostComment.Id;
                    return model;
                }
                catch (Exception)
                {
                    await transaction.RollbackAsync();
                    throw;
                }
            }

        }
public async Task<BookPostCommentModel> GetById(int id)
        {
            var BookPostComment = await _context.BookPostComments.FindAsync(id);
            if (BookPostComment == null) return null;

            return new BookPostCommentModel
            {
                Id = BookPostComment.Id,
                CommentText = BookPostComment.CommentText,
                UserId = BookPostComment.UserId,
                DateCommented = BookPostComment.DateCommented,
                BookId = BookPostComment.BookId
            };
        }
        public async Task<List<BookPostCommentModel>> GetAll()
        {
            return await _context.BookPostComments
                .Select(up => new BookPostCommentModel
                {
                    Id = up.Id,
                    CommentText = up.CommentText,
                    UserId = up.UserId,
                    DateCommented = up.DateCommented,
                    BookId = up.BookId
                })
                .ToListAsync();
        }
        public async Task<BookPostCommentModel> Update(BookPostCommentModel model)
        {
            if (model == null) throw new ArgumentNullException(nameof(model));
            var BookPostComment = await _context.BookPostComments.FindAsync(model.Id);
            if (BookPostComment == null) return null;
            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    BookPostComment.CommentText = model.CommentText;
                    BookPostComment.UserId = model.UserId;
                    BookPostComment.DateCommented = model.DateCommented;
                    BookPostComment.BookId = model.BookId;
                    _context.BookPostComments.Update(BookPostComment);
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

        public async Task<BookPostCommentModel> Delete(int id)
        {
            var BookPostComment = await _context.BookPostComments.FindAsync(id);
            if (BookPostComment == null) return null;
            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    _context.BookPostComments.Remove(BookPostComment);
                    await _context.SaveChangesAsync();
                    await transaction.CommitAsync();
                    return new BookPostCommentModel
                    {
                        Id = BookPostComment.Id,
                        CommentText = BookPostComment.CommentText,
                        UserId = BookPostComment.UserId,
                        DateCommented = BookPostComment.DateCommented,
                        BookId = BookPostComment.BookId
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
