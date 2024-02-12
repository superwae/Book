using Lafatkotob.ViewModels;

namespace Lafatkotob.Services.BookPostLikeServices
{
    public interface IBookPostLikeService
    {
        Task<ServiceResponse<BookPostLikeModel>> Post(BookPostLikeModel model);
        Task<BookPostLikeModel> GetById(int id);
        Task<List<BookPostLikeModel>> GetAll();
        Task<ServiceResponse<BookPostLikeModel>> Update(BookPostLikeModel model);
        Task<ServiceResponse<BookPostLikeModel>> Delete(int id);
    }
}
