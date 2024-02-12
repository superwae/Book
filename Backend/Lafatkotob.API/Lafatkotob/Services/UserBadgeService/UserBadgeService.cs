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

        public async Task<ServiceResponse<UserBadgeModel>> Delete(int id)
        {
            var response = new ServiceResponse<UserBadgeModel>();

            var UserBadge = await _context.UserBadges.FindAsync(id);
            if (UserBadge == null)
            {
                response.Success = false;
                response.Message = "UserBadge not found.";
                return response;
            }

            var executionStrategy = _context.Database.CreateExecutionStrategy();
            await executionStrategy.ExecuteAsync(async () =>
            {
                using (var transaction = await _context.Database.BeginTransactionAsync())
                {
                    try
                    {
                        _context.UserBadges.Remove(UserBadge);
                        await _context.SaveChangesAsync();
                        await transaction.CommitAsync();

                        response.Success = true;
                        response.Data = new UserBadgeModel
                        {
                            Id = UserBadge.Id,
                            UserId = UserBadge.UserId,
                            BadgeId = UserBadge.BadgeId,
                            DateEarned = UserBadge.DateEarned
                        };
                     
                    }
                    catch (Exception ex)
                    {
                        await transaction.RollbackAsync();
                        response.Success = false;
                        response.Message = $"Failed to delete UserBadge: {ex.Message}";
                    }
                }
            });

            return response;
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

        public async Task<ServiceResponse<UserBadgeModel>> Post(UserBadgeModel model)
        {
            var response = new ServiceResponse<UserBadgeModel>();

            var executionStrategy = _context.Database.CreateExecutionStrategy();
            await executionStrategy.ExecuteAsync(async () =>
            {
                using (var transaction = _context.Database.BeginTransaction())
                {
                    try
                    {

                        var UserBadge = new UserBadge
                        {
                            Id = model.Id,
                            UserId = model.UserId,
                            BadgeId = model.BadgeId,
                            DateEarned = DateTime.Now
                        };
                       
                        _context.UserBadges.Add(UserBadge);
                        await _context.SaveChangesAsync();

                        transaction.Commit();
                        response.Success = true;
                        response.Data = model;
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        response.Success = false;
                        response.Message = "Failed to create UserBadge.";
                    }
                }
            });

            return response;
        }

        public async Task<ServiceResponse<UserBadgeModel>> Update(UserBadgeModel model)
        {
            var response = new ServiceResponse<UserBadgeModel>();

            if (model == null)
            {
                response.Success = false;
                response.Message = "Model cannot be null.";
                return response;
            }

            var UserBadge = await _context.UserBadges.FindAsync(model.Id);
            if (UserBadge == null)
            {
                response.Success = false;
                response.Message = "UserBadge not found.";
                return response;
            }

            var executionStrategy = _context.Database.CreateExecutionStrategy();
            await executionStrategy.ExecuteAsync(async () =>
            {
                using (var transaction = await _context.Database.BeginTransactionAsync())
                {
                    try
                    {
                        UserBadge.DateEarned = model.DateEarned;


                        _context.UserBadges.Update(UserBadge);
                        await _context.SaveChangesAsync();

                        await transaction.CommitAsync();

                        response.Success = true;
                        response.Data = model;
                    }
                    catch (Exception ex)
                    {
                        await transaction.RollbackAsync();
                        response.Success = false;
                        response.Message = $"Failed to update UserBadge: {ex.Message}";
                    }
                }
            });

            return response;
        }
    
    }
}
