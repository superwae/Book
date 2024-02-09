using Lafatkotob.Entities;
using Lafatkotob.Services.GenreService;
using Lafatkotob.ViewModels;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Lafatkotob.Services.GenreService
{
    public class GenreService : IGenreService
    {
        private readonly ApplicationDbContext _context;

        public GenreService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<GenreModel> Post(GenreModel model)
        {
            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    var genre = new Genre
                    {
                        Name = model.Name
                    };

                    _context.Genres.Add(genre);
                    await _context.SaveChangesAsync();
                    await transaction.CommitAsync();

                    model.Id = genre.Id;
                    return model;
                }
                catch (Exception)
                {
                    await transaction.RollbackAsync();
                    throw;
                }
            }
        }

        public async Task<GenreModel> GetById(int id)
        {
            var genre = await _context.Genres.FindAsync(id);
            if (genre == null) return null;

            return new GenreModel
            {
                Id = genre.Id,
                Name = genre.Name
            };
        }

        public async Task<List<GenreModel>> GetAll()
        {
            return await _context.Genres
                .Select(g => new GenreModel
                {
                    Id = g.Id,
                    Name = g.Name
                })
                .ToListAsync();
        }

        public async Task<GenreModel> Update(GenreModel model)
        {
            if (model == null) throw new ArgumentNullException(nameof(model));

            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    var genre = await _context.Genres.FindAsync(model.Id);
                    if (genre == null) return null;

                    genre.Name = model.Name;

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

        public async Task<GenreModel> Delete(int id)
        {
            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    var genre = await _context.Genres.FindAsync(id);
                    if (genre == null) return null;

                    _context.Genres.Remove(genre);
                    await _context.SaveChangesAsync();
                    await transaction.CommitAsync();

                    return new GenreModel
                    {
                        Id = genre.Id,
                        Name = genre.Name
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
