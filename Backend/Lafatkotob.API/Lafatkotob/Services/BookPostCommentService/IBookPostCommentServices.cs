using Lafatkotob.ViewModels;

namespace Lafatkotob.Services.BookPostCommentService
{
    public interface IBookPostCommentServices
    {
        Task<BookPostCommentModel> Post(BookPostCommentModel model);
        Task<BookPostCommentModel> GetById(int id);
        Task<List<BookPostCommentModel>> GetAll();
        Task<BookPostCommentModel> Update(BookPostCommentModel model);
        Task<BookPostCommentModel> Delete(int id);
    }
}
