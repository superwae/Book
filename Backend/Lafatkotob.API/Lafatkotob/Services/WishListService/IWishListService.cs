using Lafatkotob.ViewModels;

namespace Lafatkotob.Services.WishListService
{
    public interface IWishListService
    {
        Task<WishlistModel> post(WishlistModel model);
        Task<WishlistModel> get(int id);
        Task<WishlistModel> put(WishlistModel model);
        Task<WishlistModel> delete(int id);

    }
}
