using Lafatkotob.ViewModels;

namespace Lafatkotob.Services.ConversationService
{
    public interface IConversationService
    {
        Task<ConversationModel> Post(ConversationModel model);
        Task<ConversationModel> GetById(int id);
        Task<List<ConversationModel>> GetAll();
        Task<ConversationModel> Update(ConversationModel model);
        Task<ConversationModel> Delete(int id);
    }
}
