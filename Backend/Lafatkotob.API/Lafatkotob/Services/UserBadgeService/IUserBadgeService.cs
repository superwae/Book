using Lafatkotob.ViewModels;

namespace Lafatkotob.Services.UserBadgeService
{
    public interface IUserBadgeService
    {
        Task<UserBadgeModel> Post(UserBadgeModel model);
        Task<UserBadgeModel> GetById(int id);
        Task<List<UserBadgeModel>> GetAll();
        Task<UserBadgeModel> Update(UserBadgeModel model);
        Task<UserBadgeModel> Delete(int id);
    }
}
