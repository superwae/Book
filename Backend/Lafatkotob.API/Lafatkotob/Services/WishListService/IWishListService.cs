using Lafatkotob.ViewModels;

namespace Lafatkotob.Services.WishListService
{
    public interface IWishListService
    {
        Task<WishlistModel> Post(WishlistModel model);
        Task<WishlistModel> GetById(int id);
        Task<List<WishlistModel>> GetAll();
        Task<WishlistModel> Update(WishlistModel model);
        Task<WishlistModel> Delete(int id);

    }
}
