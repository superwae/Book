using Lafatkotob.Entities;
using Lafatkotob.ViewModels;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Lafatkotob.Services.NotificationUserService
{
    public class NotificationUserService : INotificationUserService
    {
        private readonly ApplicationDbContext _context;

        public NotificationUserService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<ServiceResponse<NotificationUserModel>> Delete(int id)
        {
            var response = new ServiceResponse<NotificationUserModel>();

            var NotificationUser = await _context.NotificationUsers.FindAsync(id);
            if (NotificationUser == null)
            {
                response.Success = false;
                response.Message = "NotificationUser not found.";
                return response;
            }

            var executionStrategy = _context.Database.CreateExecutionStrategy();
            await executionStrategy.ExecuteAsync(async () =>
            {
                using (var transaction = await _context.Database.BeginTransactionAsync())
                {
                    try
                    {
                        _context.NotificationUsers.Remove(NotificationUser);
                        await _context.SaveChangesAsync();
                        await transaction.CommitAsync();

                        response.Success = true;
                        response.Data = new NotificationUserModel
                        {
                            Id = NotificationUser.Id,
                            UserId = NotificationUser.UserId,
                            NotificationId = NotificationUser.NotificationId
                        };
                        
                    }
                    catch (Exception ex)
                    {
                        await transaction.RollbackAsync();
                        response.Success = false;
                        response.Message = $"Failed to delete NotificationUser: {ex.Message}";
                    }
                }
            });

            return response;
        }

        public async Task<NotificationUserModel> GetById(int id)
        {
            var notificationUser = await _context.NotificationUsers
                .FirstOrDefaultAsync(nu => nu.Id == id);
            if (notificationUser == null) return null;

            return new NotificationUserModel
            {
                Id = notificationUser.Id,
                UserId = notificationUser.UserId,
                NotificationId = notificationUser.NotificationId
            };
        }

        public async Task<List<NotificationUserModel>> GetAll()
        {
            return await _context.NotificationUsers
                .Select(nu => new NotificationUserModel
                {
                    Id = nu.Id,
                    UserId = nu.UserId,
                    NotificationId = nu.NotificationId
                })
                .ToListAsync();
        }

        public async Task<ServiceResponse<NotificationUserModel>> Post(NotificationUserModel model)
        {
            var response = new ServiceResponse<NotificationUserModel>();

            var executionStrategy = _context.Database.CreateExecutionStrategy();
            await executionStrategy.ExecuteAsync(async () =>
            {
                using (var transaction = _context.Database.BeginTransaction())
                {
                    try
                    {

                        var NotificationUser = new NotificationUser
                        {
                            UserId = model.UserId,
                            NotificationId = model.NotificationId
                        };
                       
                        _context.NotificationUsers.Add(NotificationUser);
                        await _context.SaveChangesAsync();

                        transaction.Commit();
                        response.Success = true;
                        response.Data = model;
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        response.Success = false;
                        response.Message = "Failed to create NotificationUser.";
                    }
                }
            });

            return response;
        }

        public async Task<ServiceResponse<NotificationUserModel>> Update(NotificationUserModel model)
        {
            var response = new ServiceResponse<NotificationUserModel>();

            if (model == null)
            {
                response.Success = false;
                response.Message = "Model cannot be null.";
                return response;
            }

            var NotificationUser = await _context.NotificationUsers.FindAsync(model.Id);
            if (NotificationUser == null)
            {
                response.Success = false;
                response.Message = "NotificationUser not found.";
                return response;
            }

            var executionStrategy = _context.Database.CreateExecutionStrategy();
            await executionStrategy.ExecuteAsync(async () =>
            {
                using (var transaction = await _context.Database.BeginTransactionAsync())
                {
                    try
                    {
                        NotificationUser.NotificationId = model.NotificationId;
                        NotificationUser.UserId = model.UserId;

                        _context.NotificationUsers.Update(NotificationUser);
                        await _context.SaveChangesAsync();

                        await transaction.CommitAsync();

                        response.Success = true;
                        response.Data = model;
                    }
                    catch (Exception ex)
                    {
                        await transaction.RollbackAsync();
                        response.Success = false;
                        response.Message = $"Failed to update NotificationUser: {ex.Message}";
                    }
                }
            });

            return response;
        }
    
    }
}
