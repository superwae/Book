using Lafatkotob.Entities;
using Lafatkotob.Services.EventService;
using Lafatkotob.ViewModels;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Lafatkotob.Services.EventService
{
    public class EventService : IEventService
    {
        private readonly ApplicationDbContext _context;

        public EventService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<EventModel> Post(EventModel model)
        {
            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    var eventEntity = new Event
                    {
                        EventName = model.EventName,
                        Description = model.Description,
                        DateScheduled = model.DateScheduled,
                        Location = model.Location,
                        HostUserId = model.HostUserId
                    };

                    _context.Events.Add(eventEntity);
                    await _context.SaveChangesAsync();
                    await transaction.CommitAsync();

                    model.Id = eventEntity.Id;
                    return model;
                }
                catch (Exception)
                {
                    await transaction.RollbackAsync();
                    throw;
                }
            }
        }

        public async Task<EventModel> GetById(int id)
        {
            var eventEntity = await _context.Events
                .AsNoTracking()
                .FirstOrDefaultAsync(e => e.Id == id);
            if (eventEntity == null) return null;

            return new EventModel
            {
                Id = eventEntity.Id,
                EventName = eventEntity.EventName,
                Description = eventEntity.Description,
                DateScheduled = eventEntity.DateScheduled,
                Location = eventEntity.Location,
                HostUserId = eventEntity.HostUserId
            };
        }

        public async Task<List<EventModel>> GetAll()
        {
            return await _context.Events
                .Select(e => new EventModel
                {
                    Id = e.Id,
                    EventName = e.EventName,
                    Description = e.Description,
                    DateScheduled = e.DateScheduled,
                    Location = e.Location,
                    HostUserId = e.HostUserId
                })
                .ToListAsync();
        }

        public async Task<EventModel> Update(EventModel model)
        {
            if (model == null) throw new ArgumentNullException(nameof(model));

            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    var eventEntity = await _context.Events.FindAsync(model.Id);
                    if (eventEntity == null) return null;

                    eventEntity.EventName = model.EventName;
                    eventEntity.Description = model.Description;
                    eventEntity.DateScheduled = model.DateScheduled;
                    eventEntity.Location = model.Location;
                    eventEntity.HostUserId = model.HostUserId;

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

        public async Task<EventModel> Delete(int id)
        {
            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    var eventEntity = await _context.Events.FindAsync(id);
                    if (eventEntity == null) return null;

                    _context.Events.Remove(eventEntity);
                    await _context.SaveChangesAsync();
                    await transaction.CommitAsync();

                    return new EventModel
                    {
                        Id = eventEntity.Id,
                        EventName = eventEntity.EventName,
                        Description = eventEntity.Description,
                        DateScheduled = eventEntity.DateScheduled,
                        Location = eventEntity.Location,
                        HostUserId = eventEntity.HostUserId

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
