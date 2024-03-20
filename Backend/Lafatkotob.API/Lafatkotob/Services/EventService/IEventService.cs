using Lafatkotob.ViewModels;

namespace Lafatkotob.Services.EventService
{
    public interface IEventService
    {
        Task<ServiceResponse<EventModel>> Post(EventModel model, IFormFile imageFile);
        Task<ServiceResponse<EventModel>> Update(int eventId, EventModel model, IFormFile imageFile);
        Task<EventModel> GetById(int id);
        Task<List<EventModel>> GetEventsByUserId(string userId);
        Task<List<EventModel>> GetAll();
        Task<ServiceResponse<EventModel>> Delete(int id);
    }
}
