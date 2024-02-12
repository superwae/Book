﻿using Lafatkotob.Entities;
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

        public async Task<ServiceResponse<NotificationModel>> Post(NotificationModel model)
        {
            var response = new ServiceResponse<NotificationModel>();

            var executionStrategy = _context.Database.CreateExecutionStrategy();
            await executionStrategy.ExecuteAsync(async () =>
            {
                using (var transaction = _context.Database.BeginTransaction())
                {
                    try
                    {

                        var Notification = new Notification
                        {
                            Message = model.Message,
                            DateSent = DateTime.Now,
                            IsRead = model.IsRead
                        };
                        
                        _context.Notifications.Add(Notification);
                        await _context.SaveChangesAsync();

                        transaction.Commit();
                        response.Success = true;
                        response.Data = model;
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        response.Success = false;
                        response.Message = "Failed to create Notification.";
                    }
                }
            });

            return response;
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

        public async Task<ServiceResponse<NotificationModel>> Update(NotificationModel model)
        {
            var response = new ServiceResponse<NotificationModel>();

            if (model == null)
            {
                response.Success = false;
                response.Message = "Model cannot be null.";
                return response;
            }

            var Notification = await _context.Notifications.FindAsync(model.Id);
            if (Notification == null)
            {
                response.Success = false;
                response.Message = "Notification not found.";
                return response;
            }

            var executionStrategy = _context.Database.CreateExecutionStrategy();
            await executionStrategy.ExecuteAsync(async () =>
            {
                using (var transaction = await _context.Database.BeginTransactionAsync())
                {
                    try
                    {
                        Notification.IsRead = model.IsRead;
                        Notification.Message = model.Message;
                        Notification.DateSent = model.DateSent;
                        Notification.IsRead = model.IsRead;

                        _context.Notifications.Update(Notification);
                        await _context.SaveChangesAsync();

                        await transaction.CommitAsync();

                        response.Success = true;
                        response.Data = model;
                    }
                    catch (Exception ex)
                    {
                        await transaction.RollbackAsync();
                        response.Success = false;
                        response.Message = $"Failed to Notification badge: {ex.Message}";
                    }
                }
            });

            return response;
        }

        public async Task<ServiceResponse<NotificationModel>> Delete(int id)
        {
            var response = new ServiceResponse<NotificationModel>();

            var Notification = await _context.Notifications.FindAsync(id);
            if (Notification == null)
            {
                response.Success = false;
                response.Message = "Notification not found.";
                return response;
            }

            var executionStrategy = _context.Database.CreateExecutionStrategy();
            await executionStrategy.ExecuteAsync(async () =>
            {
                using (var transaction = await _context.Database.BeginTransactionAsync())
                {
                    try
                    {
                        _context.Notifications.Remove(Notification);
                        await _context.SaveChangesAsync();
                        await transaction.CommitAsync();

                        response.Success = true;
                        response.Data = new NotificationModel
                        {
                            Id = Notification.Id,
                            Message = Notification.Message,
                            DateSent = Notification.DateSent,
                            IsRead = Notification.IsRead
                        };
                      
                    }
                    catch (Exception ex)
                    {
                        await transaction.RollbackAsync();
                        response.Success = false;
                        response.Message = $"Failed to delete Notification: {ex.Message}";
                    }
                }
            });

            return response;
        }
    }
}
