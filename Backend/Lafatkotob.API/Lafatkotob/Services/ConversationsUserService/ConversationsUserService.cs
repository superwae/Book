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

        public async Task<ServiceResponse<ConversationsUserModel>> Post(ConversationsUserModel model)
        {
            var response = new ServiceResponse<ConversationsUserModel>();
            var executionStrategy = _context.Database.CreateExecutionStrategy();
            await executionStrategy.ExecuteAsync(async () =>
            {
                using (var transaction = _context.Database.BeginTransaction())
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
                        transaction.Commit();

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

        public async Task<ServiceResponse<ConversationsUserModel>> Update(ConversationsUserModel model)
        {
            var response = new ServiceResponse<ConversationsUserModel>();

            if (model == null)
            {
                response.Success = false;
                response.Message = "Model cannot be null.";
                return response;
            }
            var ConversationsUser = await _context.ConversationsUsers.FindAsync(model.Id);
            if (ConversationsUser == null)
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

                        ConversationsUser.UserId = model.UserId;
                        ConversationsUser.ConversationId = model.ConversationId;
                        _context.ConversationsUsers.Update(ConversationsUser);
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

        public async Task<ServiceResponse<ConversationsUserModel>> Delete(int id)
        {
            var response = new ServiceResponse<ConversationsUserModel>();

            var conversationsUser = await _context.ConversationsUsers.FindAsync(id);
            if (conversationsUser == null)
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


                        _context.ConversationsUsers.Remove(conversationsUser);
                        await _context.SaveChangesAsync();
                        await transaction.CommitAsync();

                        response.Success = true;
                        response.Data = new ConversationsUserModel
                        {
                            Id = conversationsUser.Id,
                            UserId = conversationsUser.UserId,
                            ConversationId = conversationsUser.ConversationId

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
