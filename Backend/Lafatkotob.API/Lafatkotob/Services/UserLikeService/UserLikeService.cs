using Lafatkotob.Entities;
using Lafatkotob.ViewModels;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Lafatkotob.Services.UserLikeService
{
    public class UserLikeService : IUserLikeService
    {
        private readonly ApplicationDbContext _context;

        public UserLikeService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<UserLikeModel> Delete(int id)
        {
            var userLike = await _context.UserLikes.FindAsync(id);
            if (userLike == null) return null;

            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    _context.UserLikes.Remove(userLike);
                    await _context.SaveChangesAsync();
                    await transaction.CommitAsync();

                    return new UserLikeModel
                    {
                        Id = userLike.Id,
                        LikedUserId = userLike.LikedUserId,
                        LikingUserId = userLike.LikingUserId,
                        DateLiked = userLike.DateLiked
                    };
                }
                catch (Exception)
                {
                    await transaction.RollbackAsync();
                    throw;
                }
            }
        }

        public async Task<UserLikeModel> GetById(int id)
        {
            var userLike = await _context.UserLikes.FindAsync(id);
            if (userLike == null) return null;

            return new UserLikeModel
            {
                Id = userLike.Id,
                LikedUserId = userLike.LikedUserId,
                LikingUserId = userLike.LikingUserId,
                DateLiked = userLike.DateLiked
            };
        }

        public async Task<List<UserLikeModel>> GetAll()
        {
            return await _context.UserLikes
                .Select(ul => new UserLikeModel
                {
                    Id = ul.Id,
                    LikedUserId = ul.LikedUserId,
                    LikingUserId = ul.LikingUserId,
                    DateLiked = ul.DateLiked
                })
                .ToListAsync();
        }

        public async Task<UserLikeModel> Post(UserLikeModel model)
        {
            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    var userLike = new UserLike
                    {
                        LikedUserId = model.LikedUserId,
                        LikingUserId = model.LikingUserId,
                        DateLiked = model.DateLiked
                    };

                    _context.UserLikes.Add(userLike);
                    await _context.SaveChangesAsync();
                    await transaction.CommitAsync();

                    model.Id = userLike.Id;
                    return model;
                }
                catch (Exception)
                {
                    await transaction.RollbackAsync();
                    throw;
                }
            }
        }

        public async Task<UserLikeModel> Update(UserLikeModel model)
        {
            if (model == null) throw new ArgumentNullException(nameof(model));

            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    var userLike = await _context.UserLikes.FindAsync(model.Id);
                    if (userLike == null) return null;

                    userLike.LikedUserId = model.LikedUserId;
                    userLike.LikingUserId = model.LikingUserId;
                    userLike.DateLiked = model.DateLiked;

                    _context.Update(userLike);
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
