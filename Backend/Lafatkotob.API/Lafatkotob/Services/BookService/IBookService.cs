using Lafatkotob.ViewModels;

namespace Lafatkotob.Services.BookService
{
    public interface IBookService
    {
        Task<BooksModel> GetById(int id);
        Task<List<BooksModel>> GetAll();
        Task<ServiceResponse<BooksModel>> Delete(int id);
        Task<ServiceResponse<BooksModel>> Post(BooksModel model, IFormFile imageFile);
        Task<ServiceResponse<UpdateBookModel>> Update(int id, UpdateBookModel model, IFormFile imageFile = null);
    }
}
