using Lafatkotob.ViewModels;

namespace Lafatkotob.Services.UserPreferenceService
{
    public interface IUserPreferenceService
    {
        Task<ServiceResponse<UserPreferenceModel>> Post(UserPreferenceModel model);
        Task<UserPreferenceModel> GetById(int id);
        Task<List<UserPreferenceModel>> GetAll();
        Task<ServiceResponse<UserPreferenceModel>> Update(UserPreferenceModel model);
        Task<ServiceResponse<UserPreferenceModel>> Delete(int id);
    }
}
