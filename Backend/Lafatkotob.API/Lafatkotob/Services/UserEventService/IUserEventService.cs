using Lafatkotob.ViewModels;

namespace Lafatkotob.Services.UserEventService
{
    public interface IUserEventService
    {
        Task<UserEventModel> Post(UserEventModel model);
        Task<UserEventModel> GetById(int id);
        Task<List<UserEventModel>> GetAll();
        Task<UserEventModel> Update(UserEventModel model);
        Task<UserEventModel> Delete(int id);
    }
}
