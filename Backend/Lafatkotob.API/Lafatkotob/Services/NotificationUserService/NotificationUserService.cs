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

        public async Task<NotificationUserModel> Delete(int id)
        {
            var notificationUser = await _context.NotificationUsers.FindAsync(id);
            if (notificationUser == null) return null;

            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    _context.NotificationUsers.Remove(notificationUser);
                    await _context.SaveChangesAsync();
                    await transaction.CommitAsync();

                    return new NotificationUserModel
                    {
                        Id = notificationUser.Id,
                        UserId = notificationUser.UserId,
                        NotificationId = notificationUser.NotificationId
                    };
                }
                catch (Exception)
                {
                    await transaction.RollbackAsync();
                    throw;
                }
            }
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

        public async Task<NotificationUserModel> Post(NotificationUserModel model)
        {
            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    var notificationUser = new NotificationUser
                    {
                        UserId = model.UserId,
                        NotificationId = model.NotificationId
                    };

                    _context.NotificationUsers.Add(notificationUser);
                    await _context.SaveChangesAsync();
                    await transaction.CommitAsync();

                    model.Id = notificationUser.Id;
                    return model;
                }
                catch (Exception)
                {
                    await transaction.RollbackAsync();
                    throw;
                }
            }
        }

        public async Task<NotificationUserModel> Update(NotificationUserModel model)
        {
            if (model == null) throw new ArgumentNullException(nameof(model));

            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    var notificationUser = await _context.NotificationUsers.FindAsync(model.Id);
                    if (notificationUser == null) return null;

                    notificationUser.UserId = model.UserId;
                    notificationUser.NotificationId = model.NotificationId;

                    _context.NotificationUsers.Update(notificationUser);
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
