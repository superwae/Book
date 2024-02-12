using Lafatkotob.ViewModels;

namespace Lafatkotob.Services.ConversationService
{
    public interface IConversationService
    {
        Task<ServiceResponse<ConversationModel>> Post(ConversationModel model);
        Task<ConversationModel> GetById(int id);
        Task<List<ConversationModel>> GetAll();
        Task<ServiceResponse<ConversationModel>> Update(ConversationModel model);
        Task<ServiceResponse<ConversationModel>>Delete(int id);
    }
}
