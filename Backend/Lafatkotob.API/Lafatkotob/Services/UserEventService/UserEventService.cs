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

        public async Task<UserEventModel> Delete(int id)
        {
            var userEvent = await _context.UserEvents.FindAsync(id);
            if (userEvent == null) return null;

            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    _context.UserEvents.Remove(userEvent);
                    await _context.SaveChangesAsync();
                    await transaction.CommitAsync();

                    return new UserEventModel
                    {
                        Id = userEvent.Id,
                        UserId = userEvent.UserId,
                        EventId = userEvent.EventId
                    };
                }
                catch (Exception)
                {
                    await transaction.RollbackAsync();
                    throw;
                }
            }
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

        public async Task<UserEventModel> Post(UserEventModel model)
        {
            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    var userEvent = new UserEvent
                    {
                        UserId = model.UserId,
                        EventId = model.EventId
                    };

                    _context.UserEvents.Add(userEvent);
                    await _context.SaveChangesAsync();
                    await transaction.CommitAsync();

                    model.Id = userEvent.Id;
                    return model;
                }
                catch (Exception)
                {
                    await transaction.RollbackAsync();
                    throw;
                }
            }
        }

        public async Task<UserEventModel> Update(UserEventModel model)
        {
            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                if (model == null) throw new ArgumentNullException(nameof(model));

                try
                {
                    var userEvent = await _context.UserEvents.FindAsync(model.Id);
                    if (userEvent == null) return null;

                    userEvent.UserId = model.UserId;
                    userEvent.EventId = model.EventId;

                    _context.UserEvents.Update(userEvent);
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
