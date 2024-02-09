using Lafatkotob.Entities;
using Lafatkotob.ViewModels;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Lafatkotob.Services.UserBadgeService
{
    public class UserBadgeService : IUserBadgeService
    {
        private readonly ApplicationDbContext _context;

        public UserBadgeService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<UserBadgeModel> Delete(int id)
        {
            var userBadge = await _context.UserBadges.FindAsync(id);
            if (userBadge == null) return null;

            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    _context.UserBadges.Remove(userBadge);
                    await _context.SaveChangesAsync();
                    await transaction.CommitAsync();

                    return new UserBadgeModel
                    {
                        Id = userBadge.Id,
                        UserId = userBadge.UserId,
                        BadgeId = userBadge.BadgeId,
                        DateEarned = userBadge.DateEarned
                    };
                }
                catch (Exception)
                {
                    await transaction.RollbackAsync();
                    throw;
                }
            }
        }

        public async Task<UserBadgeModel> GetById(int id)
        {
            var userBadge = await _context.UserBadges
                .FirstOrDefaultAsync(ub => ub.Id == id);
            if (userBadge == null) return null;

            return new UserBadgeModel
            {
                Id = userBadge.Id,
                UserId = userBadge.UserId,
                BadgeId = userBadge.BadgeId,
                DateEarned = userBadge.DateEarned
            };
        }

        public async Task<List<UserBadgeModel>> GetAll()
        {
            return await _context.UserBadges
                .Select(ub => new UserBadgeModel
                {
                    Id = ub.Id,
                    UserId = ub.UserId,
                    BadgeId = ub.BadgeId,
                    DateEarned = ub.DateEarned
                })
                .ToListAsync();
        }

        public async Task<UserBadgeModel> Post(UserBadgeModel model)
        {
            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    var userBadge = new UserBadge
                    {
                        UserId = model.UserId,
                        BadgeId = model.BadgeId,
                        DateEarned = DateTime.Now
                    };

                    _context.UserBadges.Add(userBadge);
                    await _context.SaveChangesAsync();
                    await transaction.CommitAsync();

                    model.Id = userBadge.Id;
                    return model;
                }
                catch (Exception)
                {
                    await transaction.RollbackAsync();
                    throw;
                }
            }
        }

        public async Task<UserBadgeModel> Update(UserBadgeModel model)
        {
            if (model == null) throw new ArgumentNullException(nameof(model));

            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    var userBadge = await _context.UserBadges.FindAsync(model.Id);
                    if (userBadge == null) return null;

                    userBadge.UserId = model.UserId;
                    userBadge.BadgeId = model.BadgeId;
                    userBadge.DateEarned = model.DateEarned;

                    _context.UserBadges.Update(userBadge);
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
