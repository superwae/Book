using Lafatkotob.ViewModels;

namespace Lafatkotob.Services.EventService
{
    public interface IEventService
    {
        Task<EventModel> Post(EventModel model);
        Task<EventModel> GetById(int id);
        Task<List<EventModel>> GetAll();
        Task<EventModel> Update(EventModel model);
        Task<EventModel> Delete(int id);
    }
}
