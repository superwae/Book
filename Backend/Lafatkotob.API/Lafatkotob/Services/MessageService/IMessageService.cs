using Lafatkotob.ViewModels;

namespace Lafatkotob.Services.MessageService
{
    public interface IMessageService
    {
        Task<MessageModel> Post(MessageModel model);
        Task<MessageModel> GetById(int id);
        Task<List<MessageModel>> GetAll();
        Task<MessageModel> Update(MessageModel model);
        Task<MessageModel> Delete(int id);
    }
}
