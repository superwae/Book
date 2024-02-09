using Lafatkotob.Entities;
using Lafatkotob.ViewModels;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Lafatkotob.Services.UserReviewService
{
    public class UserReviewService : IUserReviewService
    {
        private readonly ApplicationDbContext _context;

        public UserReviewService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<UserReviewModel> Delete(int id)
        {
            var userReview = await _context.UserReviews.FindAsync(id);
            if (userReview == null) return null;

            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    _context.UserReviews.Remove(userReview);
                    await _context.SaveChangesAsync();
                    await transaction.CommitAsync();

                    return new UserReviewModel
                    {
                        Id = userReview.Id,
                        ReviewedUserId = userReview.ReviewedUserId,
                        ReviewingUserId = userReview.ReviewingUserId,
                        ReviewText = userReview.ReviewText,
                        DateReviewed = userReview.DateReviewed,
                        Rating = userReview.Rating
                    };
                }
                catch (Exception)
                {
                    await transaction.RollbackAsync();
                    throw;
                }
            }
        }

        public async Task<UserReviewModel> GetById(int id)
        {
            var userReview = await _context.UserReviews.FindAsync(id);
            if (userReview == null) return null;

            return new UserReviewModel
            {
                Id = userReview.Id,
                ReviewedUserId = userReview.ReviewedUserId,
                ReviewingUserId = userReview.ReviewingUserId,
                ReviewText = userReview.ReviewText,
                DateReviewed = userReview.DateReviewed,
                Rating = userReview.Rating
            };
        }

        public async Task<List<UserReviewModel>> GetAll()
        {
            return await _context.UserReviews
                .Select(ur => new UserReviewModel
                {
                    Id = ur.Id,
                    ReviewedUserId = ur.ReviewedUserId,
                    ReviewingUserId = ur.ReviewingUserId,
                    ReviewText = ur.ReviewText,
                    DateReviewed = ur.DateReviewed,
                    Rating = ur.Rating
                })
                .ToListAsync();
        }

        public async Task<UserReviewModel> Post(UserReviewModel model)
        {
            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    var userReview = new UserReview
                    {
                        ReviewedUserId = model.ReviewedUserId,
                        ReviewingUserId = model.ReviewingUserId,
                        ReviewText = model.ReviewText,
                        DateReviewed = DateTime.Now,
                        Rating = model.Rating
                    };

                    _context.UserReviews.Add(userReview);
                    await _context.SaveChangesAsync();
                    await transaction.CommitAsync();

                    model.Id = userReview.Id;
                    return model;
                }
                catch (Exception)
                {
                    await transaction.RollbackAsync();
                    throw;
                }
            }
        }

        public async Task<UserReviewModel> Update(UserReviewModel model)
        {
            if (model == null) throw new ArgumentNullException(nameof(model));

            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    var userReview = await _context.UserReviews.FindAsync(model.Id);
                    if (userReview == null) return null;

                    userReview.ReviewedUserId = model.ReviewedUserId;
                    userReview.ReviewingUserId = model.ReviewingUserId;
                    userReview.ReviewText = model.ReviewText;
                    userReview.DateReviewed = DateTime.Now;
                    userReview.Rating = model.Rating;

                    _context.Update(userReview);
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
