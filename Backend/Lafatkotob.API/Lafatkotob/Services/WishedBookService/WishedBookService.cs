using Lafatkotob.Entities;
using Lafatkotob.ViewModels;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Lafatkotob.Services.WishedBookService
{
    public class WishedBookService : IWishedBookService
    {
        private readonly ApplicationDbContext _context;

        public WishedBookService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<WishedBookModel> Delete(int id)
        {
            var wishedBook = await _context.WishedBooks.FindAsync(id);
            if (wishedBook == null) return null;

            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    _context.WishedBooks.Remove(wishedBook);
                    await _context.SaveChangesAsync();
                    await transaction.CommitAsync();

                    return new WishedBookModel
                    {
                        Id = wishedBook.Id,
                        BooksInWishlistsId = wishedBook.BooksInWishlistsId,
                        WishlistId = wishedBook.WishlistId
                    };
                }
                catch (Exception)
                {
                    await transaction.RollbackAsync();
                    throw;
                }
            }
        }

        public async Task<WishedBookModel> GetById(int id)
        {
            var wishedBook = await _context.WishedBooks.FindAsync(id);
            if (wishedBook == null) return null;

            return new WishedBookModel
            {
                Id = wishedBook.Id,
                BooksInWishlistsId = wishedBook.BooksInWishlistsId,
                WishlistId = wishedBook.WishlistId
            };
        }

        public async Task<List<WishedBookModel>> GetAll()
        {
            return await _context.WishedBooks
                .Select(wb => new WishedBookModel
                {
                    Id = wb.Id,
                    BooksInWishlistsId = wb.BooksInWishlistsId,
                    WishlistId = wb.WishlistId
                })
                .ToListAsync();
        }

        public async Task<WishedBookModel> Post(WishedBookModel model)
        {
            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    var wishedBook = new WishedBook
                    {
                        BooksInWishlistsId = model.BooksInWishlistsId,
                        WishlistId = model.WishlistId
                    };

                    _context.WishedBooks.Add(wishedBook);
                    await _context.SaveChangesAsync();
                    await transaction.CommitAsync();

                    model.Id = wishedBook.Id;
                    return model;
                }
                catch (Exception)
                {
                    await transaction.RollbackAsync();
                    throw;
                }
            }
        }

        public async Task<WishedBookModel> Update(WishedBookModel model)
        {
            if (model == null) throw new ArgumentNullException(nameof(model));

            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    var wishedBook = await _context.WishedBooks.FindAsync(model.Id);
                    if (wishedBook == null) return null;

                    wishedBook.BooksInWishlistsId = model.BooksInWishlistsId;
                    wishedBook.WishlistId = model.WishlistId;

                    _context.Update(wishedBook);
                    await _context.SaveChangesAsync();
                    await transaction.CommitAsync();

                    return model;
                }
                catch (Exception)
                {
                    await transaction.RollbackAsync();
                    throw;
                }
            }
        }
    }
}
