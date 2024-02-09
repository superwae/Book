using Lafatkotob.Entities;
using Lafatkotob.ViewModels;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Lafatkotob.Services.UserPreferenceService
{
    public class UserPreferenceService : IUserPreferenceService
    {
        private readonly ApplicationDbContext _context;

        public UserPreferenceService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<UserPreferenceModel> Delete(int id)
        {
            var userPreference = await _context.UserPreferences.FindAsync(id);
            if (userPreference == null) return null;

            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    _context.UserPreferences.Remove(userPreference);
                    await _context.SaveChangesAsync();
                    await transaction.CommitAsync();

                    return new UserPreferenceModel
                    {
                        Id = userPreference.Id,
                        UserId = userPreference.UserId,
                        GenreId = userPreference.GenreId,
                        PreferredAuthor = userPreference.PreferredAuthor
                    };
                }
                catch (Exception)
                {
                    await transaction.RollbackAsync();
                    throw;
                }
            }
        }

        public async Task<UserPreferenceModel> GetById(int id)
        {
            var userPreference = await _context.UserPreferences.FindAsync(id);
            if (userPreference == null) return null;

            return new UserPreferenceModel
            {
                Id = userPreference.Id,
                UserId = userPreference.UserId,
                GenreId = userPreference.GenreId,
                PreferredAuthor = userPreference.PreferredAuthor
            };
        }

        public async Task<List<UserPreferenceModel>> GetAll()
        {
            return await _context.UserPreferences
                .Select(up => new UserPreferenceModel
                {
                    Id = up.Id,
                    UserId = up.UserId,
                    GenreId = up.GenreId,
                    PreferredAuthor = up.PreferredAuthor
                })
                .ToListAsync();
        }

        public async Task<UserPreferenceModel> Post(UserPreferenceModel model)
        {
            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    var userPreference = new UserPreference
                    {
                        UserId = model.UserId,
                        GenreId = model.GenreId,
                        PreferredAuthor = model.PreferredAuthor
                    };

                    _context.UserPreferences.Add(userPreference);
                    await _context.SaveChangesAsync();
                    await transaction.CommitAsync();

                    model.Id = userPreference.Id;
                    return model;
                }
                catch (Exception)
                {
                    await transaction.RollbackAsync();
                    throw;
                }
            }
        }

        public async Task<UserPreferenceModel> Update(UserPreferenceModel model)
        {
            if (model == null) throw new ArgumentNullException(nameof(model));

            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    var userPreference = await _context.UserPreferences.FindAsync(model.Id);
                    if (userPreference == null) return null;

                    userPreference.UserId = model.UserId;
                    userPreference.GenreId = model.GenreId;
                    userPreference.PreferredAuthor = model.PreferredAuthor;

                    _context.Update(userPreference);
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
