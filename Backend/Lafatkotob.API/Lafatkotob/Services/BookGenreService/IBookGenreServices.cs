using Lafatkotob.ViewModels;

namespace Lafatkotob.Services.BookGenreService
{
    public interface IBookGenreServices
    {
        Task<BookGenreModel> Post(BookGenreModel model);
        Task<BookGenreModel> GetById(int id);
        Task<List<BookGenreModel>> GetAll();
        Task<BookGenreModel> Update(BookGenreModel model);
        Task<BookGenreModel> Delete(int id);
    }
}
