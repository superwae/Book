using Lafatkotob.ViewModels;

namespace Lafatkotob.Services.EventService
{
    public interface IEventService
    {
        Task<ServiceResponse<EventModel>> Post(EventModel model);
        Task<EventModel> GetById(int id);
        Task<List<EventModel>> GetAll();
        Task<ServiceResponse<EventModel>> Update(EventModel model);
        Task<ServiceResponse<EventModel>> Delete(int id);
    }
}
