using Lafatkotob.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Lafatkotob.Services.WishedBookService
{
    public interface IWishedBookService
    {
        Task<WishedBookModel> Post(WishedBookModel model);
        Task<WishedBookModel> GetById(int id);
        Task<List<WishedBookModel>> GetAll();
        Task<WishedBookModel> Update(WishedBookModel model);
        Task<WishedBookModel> Delete(int id);
    }
}
