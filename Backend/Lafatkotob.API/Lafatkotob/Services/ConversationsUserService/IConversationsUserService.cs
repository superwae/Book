using Lafatkotob.ViewModels;

namespace Lafatkotob.Services.ConversationsUserService
{
    public interface IConversationsUserService
    {
        Task<ServiceResponse<ConversationsUserModel>> Post(ConversationsUserModel model);
        Task<ConversationsUserModel> GetById(int id);
        Task<List<ConversationsUserModel>> GetAll();
        Task<ServiceResponse<ConversationsUserModel>>Update(ConversationsUserModel model);
        Task<ServiceResponse<ConversationsUserModel>>Delete(int id);
    }
}
