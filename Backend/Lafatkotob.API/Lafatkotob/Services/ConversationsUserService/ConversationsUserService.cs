using Lafatkotob.Entities;
using Lafatkotob.Services.ConversationsUserService;
using Lafatkotob.ViewModels;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Lafatkotob.Services.ConversationsUserService
{
    public class ConversationsUserService : IConversationsUserService
    {
        private readonly ApplicationDbContext _context;

        public ConversationsUserService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<ConversationsUserModel> Post(ConversationsUserModel model)
        {
            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    var conversationsUser = new ConversationsUser
                    {
                        UserId = model.UserId,
                        ConversationId = model.ConversationId
                    };

                    _context.ConversationsUsers.Add(conversationsUser);
                    await _context.SaveChangesAsync();
                    await transaction.CommitAsync();

                    model.Id = conversationsUser.Id;
                    return model;
                }
                catch (Exception)
                {
                    await transaction.RollbackAsync();
                    throw;
                }
            }
        }

        public async Task<ConversationsUserModel> GetById(int id)
        {
            var conversationsUser = await _context.ConversationsUsers
                .AsNoTracking()
                .FirstOrDefaultAsync(cu => cu.Id == id);
            if (conversationsUser == null) return null;

            return new ConversationsUserModel
            {
                Id = conversationsUser.Id,
                UserId = conversationsUser.UserId,
                ConversationId = conversationsUser.ConversationId
            };
        }

        public async Task<List<ConversationsUserModel>> GetAll()
        {
            return await _context.ConversationsUsers
                .AsNoTracking()
                .Select(cu => new ConversationsUserModel
                {
                    Id = cu.Id,
                    UserId = cu.UserId,
                    ConversationId = cu.ConversationId
                })
                .ToListAsync();
        }

        public async Task<ConversationsUserModel> Update(ConversationsUserModel model)
        {
            if (model == null) throw new ArgumentNullException(nameof(model));
            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    var conversationsUser = await _context.ConversationsUsers.FindAsync(model.Id);
                    if (conversationsUser == null) return null;

                    conversationsUser.UserId = model.UserId;
                    conversationsUser.ConversationId = model.ConversationId;

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

        public async Task<ConversationsUserModel> Delete(int id)
        {
            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    var conversationsUser = await _context.ConversationsUsers.FindAsync(id);
                    if (conversationsUser == null) return null;

                    _context.ConversationsUsers.Remove(conversationsUser);
                    await _context.SaveChangesAsync();
                    await transaction.CommitAsync();

                    return new ConversationsUserModel
                    {
                        Id = conversationsUser.Id,
                        UserId = conversationsUser.UserId,
                        ConversationId = conversationsUser.ConversationId

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
