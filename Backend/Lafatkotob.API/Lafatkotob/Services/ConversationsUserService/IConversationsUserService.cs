using Lafatkotob.ViewModels;

namespace Lafatkotob.Services.ConversationsUserService
{
    public interface IConversationsUserService
    {
        Task<ConversationsUserModel> Post(ConversationsUserModel model);
        Task<ConversationsUserModel> GetById(int id);
        Task<List<ConversationsUserModel>> GetAll();
        Task<ConversationsUserModel> Update(ConversationsUserModel model);
        Task<ConversationsUserModel> Delete(int id);
    }
}
