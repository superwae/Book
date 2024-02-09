using Lafatkotob.ViewModels;

namespace Lafatkotob.Services.BookPostLikeServices
{
    public interface IBookPostLikeService
    {
        Task<BookPostLikeModel> Post(BookPostLikeModel model);
        Task<BookPostLikeModel> GetById(int id);
        Task<List<BookPostLikeModel>> GetAll();
        Task<BookPostLikeModel> Update(BookPostLikeModel model);
        Task<BookPostLikeModel> Delete(int id);
    }
}
