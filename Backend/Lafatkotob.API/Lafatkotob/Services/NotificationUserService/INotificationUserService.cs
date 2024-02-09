using Lafatkotob.ViewModels;

namespace Lafatkotob.Services.NotificationUserService
{
    public interface INotificationUserService
    {
        Task<NotificationUserModel> Post(NotificationUserModel model);
        Task<NotificationUserModel> GetById(int id);
        Task<List<NotificationUserModel>> GetAll();
        Task<NotificationUserModel> Update(NotificationUserModel model);
        Task<NotificationUserModel> Delete(int id);
    }
}
