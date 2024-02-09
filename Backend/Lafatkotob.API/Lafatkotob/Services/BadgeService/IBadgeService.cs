using Lafatkotob.ViewModels;

namespace Lafatkotob.Services.BadgeService
{
    public interface IBadgeService
    {
        Task<BadgeModel> Post(BadgeModel model);
        Task<BadgeModel> GetById(int id);
        Task<List<BadgeModel>> GetAll();
        Task<BadgeModel> Update(BadgeModel model);
        Task<BadgeModel> Delete(int id);
    }
}
