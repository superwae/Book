using Lafatkotob.ViewModels;

namespace Lafatkotob.Services.UserLikeService
{
    public interface IUserLikeService
    {
        Task<UserLikeModel> Post(UserLikeModel model);
        Task<UserLikeModel> GetById(int id);
        Task<List<UserLikeModel>> GetAll();
        Task<UserLikeModel> Update(UserLikeModel model);
        Task<UserLikeModel> Delete(int id);
    }
}
