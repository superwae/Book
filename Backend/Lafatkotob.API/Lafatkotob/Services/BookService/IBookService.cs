using Lafatkotob.ViewModels;

namespace Lafatkotob.Services.BookService
{
    public interface IBookService
    {
        Task<BookModel> GetById(int id);
        Task<List<BookModel>> GetAll();
        Task<ServiceResponse<BookModel>> Delete(int id);
        Task<ServiceResponse<BookModel>> Post(RegisterBook model, IFormFile imageFile);
        Task<ServiceResponse<UpdateBookModel>> Update(int id, UpdateBookModel model, IFormFile imageFile = null);
    }
}
