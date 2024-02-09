using Lafatkotob.Entities;
using Lafatkotob.Services;
using Lafatkotob.Services.NotificationService;
using Lafatkotob.ViewModels;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Lafatkotob.Services.NotificationService
{
    public class NotificationService : INotificationService
    {
        private readonly ApplicationDbContext _context;

        public NotificationService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<NotificationModel> Post(NotificationModel model)
        {
            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    var notification = new Notification
                    {
                        Message = model.Message,
                        DateSent = model.DateSent,
                        IsRead = model.IsRead
                    };

                    _context.Notifications.Add(notification);
                    await _context.SaveChangesAsync();
                    await transaction.CommitAsync();

                    model.Id = notification.Id;
                    return model;
                }
                catch (Exception)
                {
                    await transaction.RollbackAsync();
                    throw;
                }
            }
        }

        public async Task<NotificationModel> GetById(int id)
        {
            var notification = await _context.Notifications.FindAsync(id);
            if (notification == null) return null;

            return new NotificationModel
            {
                Id = notification.Id,
                Message = notification.Message,
                DateSent = notification.DateSent,
                IsRead = notification.IsRead
            };
        }

        public async Task<List<NotificationModel>> GetAll()
        {
            return await _context.Notifications
                .Select(n => new NotificationModel
                {
                    Id = n.Id,
                    Message = n.Message,
                    DateSent = n.DateSent,
                    IsRead = n.IsRead
                })
                .ToListAsync();
        }

        public async Task<NotificationModel> Update(NotificationModel model)
        {
            if (model == null) throw new ArgumentNullException(nameof(model));

            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    var notification = await _context.Notifications.FindAsync(model.Id);
                    if (notification == null) return null;

                    notification.Message = model.Message;
                    notification.DateSent = model.DateSent;
                    notification.IsRead = model.IsRead;

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

        public async Task<NotificationModel> Delete(int id)
        {
            var notification = await _context.Notifications.FindAsync(id);
            if (notification == null) return null;

            using (var transaction = await _context.Database.BeginTransactionAsync())
            {

                try
                {
                    _context.Notifications.Remove(notification);
                    await _context.SaveChangesAsync();
                    await transaction.CommitAsync();

                    return new NotificationModel
                    {
                        Id = notification.Id,
                        Message = notification.Message,
                        DateSent = notification.DateSent,
                        IsRead = notification.IsRead
                    };
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
