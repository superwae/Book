using Lafatkotob.ViewModels;

namespace Lafatkotob.Services.UserReviewService
{
    public interface IUserReviewService
    {
        Task<UserReviewModel> Post(UserReviewModel model);
        Task<UserReviewModel> GetById(int id);
        Task<List<UserReviewModel>> GetAll();
        Task<UserReviewModel> Update(UserReviewModel model);
        Task<UserReviewModel> Delete(int id);
    }
}
