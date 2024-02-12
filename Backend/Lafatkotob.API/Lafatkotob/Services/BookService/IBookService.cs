using Lafatkotob.ViewModels;

namespace Lafatkotob.Services.BookService
{
    public interface IBookService
    {
        Task<ServiceResponse<BooksModel>> Post(BooksModel model);
        Task<BooksModel> GetById(int id);
        Task<List<BooksModel>> GetAll();
        Task<ServiceResponse<BooksModel>> Update(BooksModel model);
        Task<ServiceResponse<BooksModel>> Delete(int id);
    }
}
