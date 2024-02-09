using Lafatkotob.Entities;
using Lafatkotob.ViewModels;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace Lafatkotob.Services.WishListService
{
    public class WishListService : IWishListService
    {
        private readonly ApplicationDbContext _context;

        public WishListService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<WishlistModel> delete(int id)
        {
            var wishlist = await _context.Wishlists.FindAsync(id);
            if (wishlist == null) return null;

            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    _context.Wishlists.Remove(wishlist);
                    await _context.SaveChangesAsync();
                    await transaction.CommitAsync();

                    return new WishlistModel
                    {
                        Id = wishlist.Id,
                        UserId = wishlist.UserId,
                        DateAdded = wishlist.DateAdded
                    };
                }
                catch (Exception)
                {
                    throw;
                }
            }
        }

        public async Task<WishlistModel> get(int id)
        {
            var wishlist = await _context.Wishlists.FindAsync(id);
            if (wishlist == null) return null;

            return new WishlistModel
            {
                Id = wishlist.Id,
                UserId = wishlist.UserId,
                DateAdded = wishlist.DateAdded,
            };
        }

        public async Task<WishlistModel> post(WishlistModel model)
        {
            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    var wishlist = new Wishlist
                    {
                        UserId = model.UserId,
                        DateAdded = model.DateAdded,
                    };

                    _context.Wishlists.Add(wishlist);
                    await _context.SaveChangesAsync();
                    await transaction.CommitAsync();

                    model.Id = wishlist.Id;
                    return model;
                }
                catch (Exception)
                {
                    throw;
                }
            }
        }

        public async Task<WishlistModel> put(WishlistModel model)
        {
            if (model == null) throw new ArgumentNullException(nameof(model));

            var wishlist = await _context.Wishlists.FindAsync(model.Id);
            if (wishlist == null) return null;

            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    wishlist.UserId = model.UserId;
                    wishlist.DateAdded = model.DateAdded;

                    _context.Wishlists.Update(wishlist);
                    await _context.SaveChangesAsync();
                    await transaction.CommitAsync();

                    return model;
                }
                catch (Exception)
                {
                    throw;
                }
            }
        }
    }
}
