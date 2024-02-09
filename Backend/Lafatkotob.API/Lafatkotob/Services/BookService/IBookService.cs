using Lafatkotob.ViewModels;

namespace Lafatkotob.Services.BookService
{
    public interface IBookService
    {
        Task<BooksModel> Post(BooksModel model);
        Task<BooksModel> GetById(int id);
        Task<List<BooksModel>> GetAll();
        Task<BooksModel> Update(BooksModel model);
        Task<BooksModel> Delete(int id);
    }
}
