using Lafatkotob.ViewModels;

namespace Lafatkotob.Services.UserPreferenceService
{
    public interface IUserPreferenceService
    {
        Task<UserPreferenceModel> Post(UserPreferenceModel model);
        Task<UserPreferenceModel> GetById(int id);
        Task<List<UserPreferenceModel>> GetAll();
        Task<UserPreferenceModel> Update(UserPreferenceModel model);
        Task<UserPreferenceModel> Delete(int id);
    }
}
