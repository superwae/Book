using Lafatkotob.Entities;
using Lafatkotob.Services.HistoryService;
using Lafatkotob.ViewModels;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Lafatkotob.Services.HistoryService
{
    public class HistoryService : IHistoryService
    {
        private readonly ApplicationDbContext _context;

        public HistoryService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<HistoryModel> Post(HistoryModel model)
        {
            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    var history = new History
                    {
                        UserId = model.UserId,
                        Date = model.Date,
                        Type = model.Type,
                        State = model.State
                    };

                    _context.History.Add(history);
                    await _context.SaveChangesAsync();

                    await transaction.CommitAsync();

                    model.Id = history.Id;
                    return model;
                }
                catch (Exception)
                {
                    await transaction.RollbackAsync();
                    throw;
                }
            }
        }

        public async Task<HistoryModel> GetById(int id)
        {
            var history = await _context.History.FindAsync(id);
            if (history == null) return null;

            return new HistoryModel
            {
                Id = history.Id,
                UserId = history.UserId,
                Date = history.Date,
                Type = history.Type,
                State = history.State
            };
        }

        public async Task<List<HistoryModel>> GetAll()
        {
            return await _context.History
                .Select(h => new HistoryModel
                {
                    Id = h.Id,
                    UserId = h.UserId,
                    Date = h.Date,
                    Type = h.Type,
                    State = h.State
                })
                .ToListAsync();
        }

        public async Task<HistoryModel> Update(HistoryModel model)
        {
            if (model == null) throw new ArgumentNullException(nameof(model));

            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    var history = await _context.History.FindAsync(model.Id);
                    if (history == null) return null;

                    history.UserId = model.UserId;
                    history.Date = model.Date;
                    history.Type = model.Type;
                    history.State = model.State;

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

        public async Task<HistoryModel> Delete(int id)
        {
            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    var history = await _context.History.FindAsync(id);
                    if (history == null) return null;

                    _context.History.Remove(history);
                    await _context.SaveChangesAsync();
                    await transaction.CommitAsync();

                    return new HistoryModel
                    {
                        Id = history.Id,
                        UserId = history.UserId,
                        Date = history.Date,
                        Type = history.Type,
                        State = history.State
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
