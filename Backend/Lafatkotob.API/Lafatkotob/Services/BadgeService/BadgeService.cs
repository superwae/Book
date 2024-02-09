using Lafatkotob.Entities;
using Lafatkotob.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace Lafatkotob.Services.BadgeService
{
    public class BadgeService : IBadgeService
    {
        private readonly ApplicationDbContext _context;
        public BadgeService(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<BadgeModel> Post(BadgeModel model)
        {
            using (var transaction = await _context.Database.BeginTransactionAsync())
            {   
                try
                {
                    var Badge = new Badge
                    {
                        Id = model.Id,
                        BadgeName = model.BadgeName,
                        Description = model.Description
                    };

                    _context.Badges.Add(Badge);
                    await _context.SaveChangesAsync();
                    await transaction.CommitAsync();

                    model.Id = Badge.Id;
                    return model;
                }
                catch (Exception)
                {
                    await transaction.RollbackAsync();
                    throw;
                }
            }
        }
       
        public async Task<BadgeModel> GetById(int id)
        {
            var badge = await _context.Badges.FindAsync(id);
            if (badge == null) return null;

            return new BadgeModel
            {
                Id = badge.Id,
                BadgeName = badge.BadgeName,
                Description = badge.Description
            };
            
        }
        public async Task<List<BadgeModel>> GetAll()
        {
            return await _context.Badges
                .Select(up => new BadgeModel
                {
                    Id = up.Id,
                    BadgeName = up.BadgeName,
                    Description = up.Description
                })
                .ToListAsync();
        }
        public async Task<BadgeModel> Update(BadgeModel model)
        {
            if (model == null) throw new ArgumentNullException(nameof(model));

            var Badges = await _context.Badges.FindAsync(model.Id);
            if (Badges == null) return null;

            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    Badges.Id = model.Id;
                    Badges.BadgeName = model.BadgeName;
                    Badges.Description = model.Description;

                    _context.Badges.Update(Badges);
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
        public async Task<BadgeModel> Delete(int id)
        {
            var Badges = await _context.Badges.FindAsync(id);
            if (Badges == null) return null;

            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    _context.Badges.Remove(Badges);
                    await _context.SaveChangesAsync();
                    await transaction.CommitAsync();

                    return new BadgeModel
                    {
                        Id = Badges.Id,
                        BadgeName = Badges.BadgeName,
                        Description = Badges.Description
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
