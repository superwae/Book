using Lafatkotob.ViewModels;

namespace Lafatkotob.Services.BooksInWishlistsService
{
    public interface IBooksInWishlistsService
    {
        Task<BookInWishlistsModel> Post(BookInWishlistsModel model);
        Task<BookInWishlistsModel> GetById(int id);
        Task<List<BookInWishlistsModel>> GetAll();
        Task<BookInWishlistsModel> Update(BookInWishlistsModel model);
        Task<BookInWishlistsModel> Delete(int id);
    }
}
