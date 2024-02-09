using Lafatkotob.ViewModels;

namespace Lafatkotob.Services.NotificationService
{
    public interface INotificationService
    {
        Task<NotificationModel> Post(NotificationModel model);
        Task<NotificationModel> GetById(int id);
        Task<List<NotificationModel>> GetAll();
        Task<NotificationModel> Update(NotificationModel model);
        Task<NotificationModel> Delete(int id);
    }
}
