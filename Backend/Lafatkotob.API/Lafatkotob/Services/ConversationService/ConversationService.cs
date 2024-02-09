using Lafatkotob.Entities;
using Lafatkotob.Services.ConversationService;
using Lafatkotob.ViewModels;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Lafatkotob.Services.ConversationService
{
    public class ConversationService : IConversationService
    {
        private readonly ApplicationDbContext _context;

        public ConversationService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<ConversationModel> Post(ConversationModel model)
        {
            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    var conversation = new Conversation
                    {
                        LastMessageDate = model.LastMessageDate
                    };

                    _context.Conversations.Add(conversation);
                    await _context.SaveChangesAsync();
                    await transaction.CommitAsync();

                    model.Id = conversation.Id;
                    return model;
                }
                catch (Exception)
                {
                    await transaction.RollbackAsync();
                    throw;
                }
            }
        }

        public async Task<ConversationModel> GetById(int id)
        {
            var conversation = await _context.Conversations.FindAsync(id);
            if (conversation == null) return null;

            return new ConversationModel
            {
                Id = conversation.Id,
                LastMessageDate = conversation.LastMessageDate
            };
        }

        public async Task<List<ConversationModel>> GetAll()
        {
            return await _context.Conversations
                .AsNoTracking()
                .Select(c => new ConversationModel
                {
                    Id = c.Id,
                    LastMessageDate = c.LastMessageDate
                })
                .ToListAsync();
        }

        public async Task<ConversationModel> Update(ConversationModel model)
        {
            if (model == null) throw new ArgumentNullException(nameof(model));

            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    var conversation = await _context.Conversations.FindAsync(model.Id);
                    if (conversation == null) return null;

                    conversation.LastMessageDate = model.LastMessageDate;

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

        public async Task<ConversationModel> Delete(int id)
        {
            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    var conversation = await _context.Conversations.FindAsync(id);
                    if (conversation == null) return new ConversationModel();

                    _context.Conversations.Remove(conversation);
                    await _context.SaveChangesAsync();
                    await transaction.CommitAsync();

                    return new ConversationModel
                    {
                        Id = conversation.Id,
                        LastMessageDate = conversation.LastMessageDate
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
