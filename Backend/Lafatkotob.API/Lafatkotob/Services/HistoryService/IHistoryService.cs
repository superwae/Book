using Lafatkotob.ViewModels;

namespace Lafatkotob.Services.HistoryService
{
    public interface IHistoryService
    {
        Task<HistoryModel> Post(HistoryModel model);
        Task<HistoryModel> GetById(int id);
        Task<List<HistoryModel>> GetAll();
        Task<HistoryModel> Update(HistoryModel model);
        Task<HistoryModel> Delete(int id);
    }
}
