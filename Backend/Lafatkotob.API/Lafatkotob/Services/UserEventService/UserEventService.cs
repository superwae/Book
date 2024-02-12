using Lafatkotob.Entities;
using Lafatkotob.ViewModels;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Lafatkotob.Services.UserEventService
{
    public class UserEventService : IUserEventService
    {
        private readonly ApplicationDbContext _context;

        public UserEventService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<ServiceResponse<UserEventModel>> Delete(int id)
        {
            var response = new ServiceResponse<UserEventModel>();

            var UserEvent = await _context.UserEvents.FindAsync(id);
            if (UserEvent == null)
            {
                response.Success = false;
                response.Message = "UserEvent not found.";
                return response;
            }

            var executionStrategy = _context.Database.CreateExecutionStrategy();
            await executionStrategy.ExecuteAsync(async () =>
            {
                using (var transaction = await _context.Database.BeginTransactionAsync())
                {
                    try
                    {
                        _context.UserEvents.Remove(UserEvent);
                        await _context.SaveChangesAsync();
                        await transaction.CommitAsync();

                        response.Success = true;
                        response.Data = new UserEventModel
                        {
                            Id = UserEvent.Id,
                            UserId = UserEvent.UserId,
                            EventId = UserEvent.EventId
                        };
                        
                    }
                    catch (Exception ex)
                    {
                        await transaction.RollbackAsync();
                        response.Success = false;
                        response.Message = $"Failed to delete UserEvent: {ex.Message}";
                    }
                }
            });

            return response;
        }

        public async Task<UserEventModel> GetById(int id)
        {
            var userEvent = await _context.UserEvents
                .FirstOrDefaultAsync(ue => ue.Id == id);
            if (userEvent == null) return null;

            return new UserEventModel
            {
                Id = userEvent.Id,
                UserId = userEvent.UserId,
                EventId = userEvent.EventId
            };
        }

        public async Task<List<UserEventModel>> GetAll()
        {
            return await _context.UserEvents
                .Select(ue => new UserEventModel
                {
                    Id = ue.Id,
                    UserId = ue.UserId,
                    EventId = ue.EventId
                })
                .ToListAsync();
        }

        public async Task<ServiceResponse<UserEventModel>> Post(UserEventModel model)
        {
            var response = new ServiceResponse<UserEventModel>();

            var executionStrategy = _context.Database.CreateExecutionStrategy();
            await executionStrategy.ExecuteAsync(async () =>
            {
                using (var transaction = _context.Database.BeginTransaction())
                {
                    try
                    {

                        var UserEvent = new UserEvent
                        {
                            UserId = model.UserId,
                            EventId = model.EventId
                        };
                        
                        _context.UserEvents.Add(UserEvent);
                        await _context.SaveChangesAsync();

                        transaction.Commit();
                        response.Success = true;
                        response.Data = model;
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        response.Success = false;
                        response.Message = "Failed to create UserEvent.";
                    }
                }
            });

            return response;
        }

        public async Task<ServiceResponse<UserEventModel>> Update(UserEventModel model)
        {
            var response = new ServiceResponse<UserEventModel>();

            if (model == null)
            {
                response.Success = false;
                response.Message = "Model cannot be null.";
                return response;
            }

            var UserEvent = await _context.UserEvents.FindAsync(model.Id);
            if (UserEvent == null)
            {
                response.Success = false;
                response.Message = "UserEvent not found.";
                return response;
            }

            var executionStrategy = _context.Database.CreateExecutionStrategy();
            await executionStrategy.ExecuteAsync(async () =>
            {
                using (var transaction = await _context.Database.BeginTransactionAsync())
                {
                    try
                    {
                        UserEvent.EventId = model.EventId;
                        UserEvent.UserId = model.UserId;


                        _context.UserEvents.Update(UserEvent);
                        await _context.SaveChangesAsync();

                        await transaction.CommitAsync();

                        response.Success = true;
                        response.Data = model;
                    }
                    catch (Exception ex)
                    {
                        await transaction.RollbackAsync();
                        response.Success = false;
                        response.Message = $"Failed to update UserEvent: {ex.Message}";
                    }
                }
            });

            return response;
        }
    }
}
