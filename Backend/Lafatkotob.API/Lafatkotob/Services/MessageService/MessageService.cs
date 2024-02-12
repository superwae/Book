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

        public async Task<ServiceResponse<MessageModel>> Post(MessageModel model)
        {
            var response = new ServiceResponse<MessageModel>();

            var executionStrategy = _context.Database.CreateExecutionStrategy();
            await executionStrategy.ExecuteAsync(async () =>
            {
                using (var transaction = _context.Database.BeginTransaction())
                {
                    try
                    {

                        var Message = new Message
                        {
                            ConversationId = model.ConversationId,
                            SenderUserId = model.SenderUserId,
                            ReceiverUserId = model.ReceiverUserId,
                            MessageText = model.MessageText,
                            DateSent = DateTime.Now,
                            IsReceived = model.IsReceived,
                            IsRead = model.IsRead,
                            IsDeletedBySender = model.IsDeletedBySender,
                            IsDeletedByReceiver = model.IsDeletedByReceiver
                        };
                       
                        _context.Messages.Add(Message);
                        await _context.SaveChangesAsync();

                        transaction.Commit();
                        response.Success = true;
                        response.Data = model;
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        response.Success = false;
                        response.Message = "Failed to create Message.";
                    }
                }
            });

            return response;
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

        public async Task<ServiceResponse<MessageModel>> Update(MessageModel model)
        {
            var response = new ServiceResponse<MessageModel>();

            if (model == null)
            {
                response.Success = false;
                response.Message = "Model cannot be null.";
                return response;
            }

            var Message = await _context.Messages.FindAsync(model.Id);
            if (Message == null)
            {
                response.Success = false;
                response.Message = "Message not found.";
                return response;
            }

            var executionStrategy = _context.Database.CreateExecutionStrategy();
            await executionStrategy.ExecuteAsync(async () =>
            {
                using (var transaction = await _context.Database.BeginTransactionAsync())
                {
                    try
                    {
                        Message.DateSent = model.DateSent;
                        Message.IsReceived = model.IsReceived;
                        Message.IsRead = model.IsRead;
                        Message.IsDeletedBySender = model.IsDeletedBySender;
                        Message.IsDeletedByReceiver = model.IsDeletedByReceiver;
                        Message.IsRead = model.IsRead;
                        Message.MessageText = model.MessageText;
                        Message.ReceiverUserId = model.ReceiverUserId;
                        Message.SenderUserId = model.SenderUserId;
                        Message.ConversationId = model.ConversationId;

                        _context.Messages.Update(Message);
                        await _context.SaveChangesAsync();

                        await transaction.CommitAsync();

                        response.Success = true;
                        response.Data = model;
                    }
                    catch (Exception ex)
                    {
                        await transaction.RollbackAsync();
                        response.Success = false;
                        response.Message = $"Message to update badge: {ex.Message}";
                    }
                }
            });

            return response;
        }
        public async Task<ServiceResponse<MessageModel>> Delete(int id)
        {
            var response = new ServiceResponse<MessageModel>();

            var Message = await _context.Messages.FindAsync(id);
            if (Message == null)
            {
                response.Success = false;
                response.Message = "Message not found.";
                return response;
            }

            var executionStrategy = _context.Database.CreateExecutionStrategy();
            await executionStrategy.ExecuteAsync(async () =>
            {
                using (var transaction = await _context.Database.BeginTransactionAsync())
                {
                    try
                    {
                        _context.Messages.Remove(Message);
                        await _context.SaveChangesAsync();
                        await transaction.CommitAsync();

                        response.Success = true;
                        response.Data = new MessageModel
                        {
                            Id = Message.Id,
                            ConversationId = Message.ConversationId,
                            SenderUserId = Message.SenderUserId,
                            ReceiverUserId = Message.ReceiverUserId,
                            MessageText = Message.MessageText,
                            DateSent = Message.DateSent,
                            IsReceived = Message.IsReceived,
                            IsRead = Message.IsRead,
                            IsDeletedBySender = Message.IsDeletedBySender,
                            IsDeletedByReceiver = Message.IsDeletedByReceiver
                        };
                        
                    }
                    catch (Exception ex)
                    {
                        await transaction.RollbackAsync();
                        response.Success = false;
                        response.Message = $"Failed to delete Message: {ex.Message}";
                    }
                }
            });

            return response;
        }
    
    }
}
