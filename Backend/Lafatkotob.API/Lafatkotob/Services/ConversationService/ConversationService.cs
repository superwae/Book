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

        public async Task<ServiceResponse<ConversationModel>> Post(ConversationModel model)
        {
            var response = new ServiceResponse<ConversationModel>();

            var executionStrategy = _context.Database.CreateExecutionStrategy();
            await executionStrategy.ExecuteAsync(async () =>
            {
                using (var transaction = _context.Database.BeginTransaction())
                {
                    try
                    {
                        var conversation = new Conversation
                        {
                            LastMessageDate = model.LastMessageDate,
                            LastMessage = model.LastMessage

                        };

                        _context.Conversations.Add(conversation);
                        await _context.SaveChangesAsync();
                        transaction.Commit();

                        model.Id = conversation.Id;
                        response.Success = true;
                        response.Data = model;
                    }
                    catch (Exception )
                    {
                        transaction.Rollback();
                        response.Success = false;
                        response.Message = "Failed to create badge.";
                    }
                }
            });
            return response;
        }

        public async Task<ConversationModel> GetById(int id)
        {
            var conversation = await _context.Conversations.FindAsync(id);
            if (conversation == null) return null;

            return new ConversationModel
            {
                Id = conversation.Id,
                LastMessageDate = conversation.LastMessageDate,
                LastMessage = conversation.LastMessage
            };
        }

        public async Task<List<ConversationModel>> GetAll()
        {
            return await _context.Conversations
                .AsNoTracking()
                .Select(c => new ConversationModel
                {
                    Id = c.Id,
                    LastMessageDate = c.LastMessageDate,
                    LastMessage = c.LastMessage

                })
                .ToListAsync();
        }

        public async Task<ServiceResponse<ConversationModel>> Update(ConversationModel model)
        {
            var response = new ServiceResponse<ConversationModel>();

            if (model == null) {
                response.Success = false;
                response.Message = "Model is null";
                return response;
            }
            var Conversation = await _context.Conversations.FindAsync(model.Id);
            if (Conversation == null)
            {
                response.Success = false;
                response.Message = "Badge not found.";
                return response;
            }
            var executionStrategy = _context.Database.CreateExecutionStrategy();
            await executionStrategy.ExecuteAsync(async () =>
            {
                using (var transaction = await _context.Database.BeginTransactionAsync())
                {
                    try
                    {
                       
                        Conversation.LastMessageDate = model.LastMessageDate;
                        Conversation.LastMessage = model.LastMessage;
                       
                        _context.Conversations.Update(Conversation);
                        await _context.SaveChangesAsync();
                        await transaction.CommitAsync();

                        response.Success = true;
                        response.Data = model;
                    }
                    catch (Exception ex)
                    {
                        await transaction.RollbackAsync();
                        response.Success = false;
                        response.Message = $"Failed to update badge: {ex.Message}";
                    }
                }
            });
            return response;
        }

        public async Task<ServiceResponse<ConversationModel>> Delete(int id)
        {
            var response = new ServiceResponse<ConversationModel>();
            var Conversation = await _context.Conversations.FindAsync(id);
            if (Conversation == null)
            {
                response.Success = false;
                response.Message = "Badge not found.";
                return response;
            }

            var executionStrategy = _context.Database.CreateExecutionStrategy();
            await executionStrategy.ExecuteAsync(async () =>
            {
                using (var transaction = await _context.Database.BeginTransactionAsync())
                {
                    try
                    {


                        _context.Conversations.Remove(Conversation);
                        await _context.SaveChangesAsync();
                        await transaction.CommitAsync();

                        response.Success = true;
                        response.Data = new ConversationModel
                        {
                            Id = Conversation.Id,
                            LastMessageDate = Conversation.LastMessageDate,
                            LastMessage = Conversation.LastMessage  
                        };
                    }
                    catch (Exception ex)
                    {
                        await transaction.RollbackAsync();
                        response.Success = false;
                        response.Message = $"Failed to delete badge: {ex.Message}";
                    }
                }
            });
            return response;
        }
    }
}
