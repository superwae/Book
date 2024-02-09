using Lafatkotob.Entities;
using Lafatkotob.Services.MessageService;
using Lafatkotob.ViewModels;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Lafatkotob.Services.MessageService
{
    public class MessageService : IMessageService
    {
        private readonly ApplicationDbContext _context;

        public MessageService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<MessageModel> Post(MessageModel model)
        {
            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    var message = new Message
                    {
                        ConversationId = model.ConversationId,
                        SenderUserId = model.SenderUserId,
                        ReceiverUserId = model.ReceiverUserId,
                        MessageText = model.MessageText,
                        DateSent = DateTime.Now,
                        IsReceived = false,
                        IsRead = false,
                        IsDeletedBySender = false,
                        IsDeletedByReceiver = false
                    };

                    _context.Messages.Add(message);
                    await _context.SaveChangesAsync();
                    await transaction.CommitAsync();

                    model.Id = message.Id;
                    return model;
                }
                catch (Exception)
                {
                    await transaction.RollbackAsync();
                    throw;
                }
            }
        }

        public async Task<MessageModel> GetById(int id)
        {
            var message = await _context.Messages
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.Id == id);
            if (message == null) return null;

            return new MessageModel
            {
                Id = message.Id,
                ConversationId = message.ConversationId,
                SenderUserId = message.SenderUserId,
                ReceiverUserId = message.ReceiverUserId,
                MessageText = message.MessageText,
                DateSent = message.DateSent,
                IsReceived = message.IsReceived,
                IsRead = message.IsRead,
                IsDeletedBySender = message.IsDeletedBySender,
                IsDeletedByReceiver = message.IsDeletedByReceiver
            };
        }

        public async Task<List<MessageModel>> GetAll()
        {
            return await _context.Messages
                .AsNoTracking()
                .Select(m => new MessageModel
                {
                    Id = m.Id,
                    ConversationId = m.ConversationId,
                    SenderUserId = m.SenderUserId,
                    ReceiverUserId = m.ReceiverUserId,
                    MessageText = m.MessageText,
                    DateSent = m.DateSent,
                    IsReceived = m.IsReceived,
                    IsRead = m.IsRead,
                    IsDeletedBySender = m.IsDeletedBySender,
                    IsDeletedByReceiver = m.IsDeletedByReceiver
                })
                .ToListAsync();
        }

        public async Task<MessageModel> Update(MessageModel model)
        {
            if (model == null) throw new ArgumentNullException(nameof(model));

            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    var existingMessage = await _context.Messages.FindAsync(model.Id);
                    if (existingMessage == null) return null;

                    existingMessage.MessageText = model.MessageText;
                    existingMessage.DateSent = model.DateSent; 
                    existingMessage.IsReceived = model.IsReceived;
                    existingMessage.IsRead = model.IsRead;
                    existingMessage.IsDeletedBySender = model.IsDeletedBySender;
                    existingMessage.IsDeletedByReceiver = model.IsDeletedByReceiver;

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

        public async Task<MessageModel> Delete(int id)
        {
            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    var message = await _context.Messages.FindAsync(id);
                    if (message == null) return null;

                    _context.Messages.Remove(message);
                    await _context.SaveChangesAsync();
                    await transaction.CommitAsync();

                    return new MessageModel
                    {
                        Id = message.Id,
                        ConversationId = message.ConversationId,
                        SenderUserId = message.SenderUserId,
                        ReceiverUserId = message.ReceiverUserId,
                        MessageText = message.MessageText,
                        DateSent = message.DateSent,
                        IsReceived = message.IsReceived,
                        IsRead = message.IsRead,
                        IsDeletedBySender = message.IsDeletedBySender,
                        IsDeletedByReceiver = message.IsDeletedByReceiver


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
