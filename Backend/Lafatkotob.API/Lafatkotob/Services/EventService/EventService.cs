using Lafatkotob.Entities;
using Lafatkotob.Services.EventService;
using Lafatkotob.ViewModels;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Lafatkotob.Services.EventService
{
    public class EventService : IEventService
    {
        private readonly ApplicationDbContext _context;

        public EventService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<ServiceResponse<EventModel>> Post(EventModel model, IFormFile imageFile)
        {
            var response = new ServiceResponse<EventModel>();

            var executionStrategy = _context.Database.CreateExecutionStrategy();
            await executionStrategy.ExecuteAsync(async () =>
            {
                using (var transaction = _context.Database.BeginTransaction())
                {
                    try
                    {
                        if (imageFile != null && imageFile.Length > 0)
                        {
                            model.ImagePath = await SaveImageAsync(imageFile);

                        }
                        var Event = new Event
                        {
                            EventName = model.EventName,
                            Description = model.Description,
                            DateScheduled = model.DateScheduled,
                            ImagePath = model.ImagePath,
                            Location = model.Location,
                            HostUserId = model.HostUserId,
                            attendances = 0
                        };
                       
                        _context.Events.Add(Event);
                        await _context.SaveChangesAsync();

                        transaction.Commit();
                        response.Success = true;
                        response.Data = model;
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        response.Success = false;
                        response.Message = "Failed to create Event.";
                    }
                }
            });

            return response;
        }

        public async Task<EventModel> GetById(int id)
        {
            var eventEntity = await _context.Events
                .AsNoTracking()
                .FirstOrDefaultAsync(e => e.Id == id);
            if (eventEntity == null) return null;

            return new EventModel
            {
                Id = eventEntity.Id,
                EventName = eventEntity.EventName,
                Description = eventEntity.Description,
                DateScheduled = eventEntity.DateScheduled,
                Location = eventEntity.Location,
                HostUserId = eventEntity.HostUserId,
                ImagePath = ConvertToFullUrl(eventEntity.ImagePath),
                attendances = eventEntity.attendances
            };
        }

        public async Task<List<EventModel>> GetEventsByUserId(string userId)
        {
            var eventModels = await _context.UserEvents
                .Where(ue => ue.UserId == userId)
                .Select(ue => new EventModel
                {
                    Id = ue.Event.Id,
                    EventName = ue.Event.EventName,
                    Description = ue.Event.Description,
                    DateScheduled = ue.Event.DateScheduled,
                    Location = ue.Event.Location,
                    HostUserId = ue.Event.HostUserId,
                    ImagePath = ConvertToFullUrl(ue.Event.ImagePath),
                    attendances = ue.Event.attendances

                })
                .ToListAsync();

            return eventModels;
        }

        public async Task<List<EventModel>> GetAll()
        {
            return await _context.Events
                .Select(e => new EventModel
                {
                    Id = e.Id,
                    EventName = e.EventName,
                    Description = e.Description,
                    DateScheduled = e.DateScheduled,
                    Location = e.Location,
                    HostUserId = e.HostUserId,
                    ImagePath = ConvertToFullUrl(e.ImagePath),
                    attendances = e.attendances

                })
                .ToListAsync();
        }

        public async Task<ServiceResponse<EventModel>> Update(int eventId, EventModel model, IFormFile imageFile)
        {
            var response = new ServiceResponse<EventModel>();

            if (model == null)
            {
                response.Success = false;
                response.Message = "Model cannot be null.";
                return response;
            }

            var Event = await _context.Events.FindAsync(model.Id);
            if (Event == null)
            {
                response.Success = false;
                response.Message = "Event not found.";
                return response;
            }
            if (imageFile != null && imageFile.Length > 0)
            {
                var imagePath = await SaveImageAsync(imageFile); 
                Event.ImagePath = imagePath; 
            }

            var executionStrategy = _context.Database.CreateExecutionStrategy();
            await executionStrategy.ExecuteAsync(async () =>
            {
                using (var transaction = await _context.Database.BeginTransactionAsync())
                {
                    try
                    {
                        Event.Location = model.Location;
                        Event.Description = model.Description;
                        Event.DateScheduled = model.DateScheduled;
                        Event.EventName = model.EventName;
                        Event.HostUserId = model.HostUserId;
                        Event.attendances = model.attendances;
                        Event.ImagePath = model.ImagePath;


                        _context.Events.Update(Event);
                        await _context.SaveChangesAsync();

                        await transaction.CommitAsync();

                        response.Success = true;
                        response.Data = model;
                    }
                    catch (Exception ex)
                    {
                        await transaction.RollbackAsync();
                        response.Success = false;
                        response.Message = $"Failed to update Event: {ex.Message}";
                    }
                }
            });

            return response;
        }

        public async Task<ServiceResponse<EventModel>> Delete(int id)
        {
            var response = new ServiceResponse<EventModel>();

            var Event = await _context.Events.FindAsync(id);
            if (Event == null)
            {
                response.Success = false;
                response.Message = "Event not found.";
                return response;
            }

            var executionStrategy = _context.Database.CreateExecutionStrategy();
            await executionStrategy.ExecuteAsync(async () =>
            {
                using (var transaction = await _context.Database.BeginTransactionAsync())
                {
                    try
                    {
                        _context.Events.Remove(Event);
                        await _context.SaveChangesAsync();
                        await transaction.CommitAsync();

                        response.Success = true;
                        response.Data = new EventModel
                        {
                            Id = Event.Id,
                            EventName = Event.EventName,
                            Description = Event.Description,
                            DateScheduled = Event.DateScheduled,
                            Location = Event.Location,
                            HostUserId = Event.HostUserId,
                            attendances = Event.attendances
                        };
                        
                    }
                    catch (Exception ex)
                    {
                        await transaction.RollbackAsync();
                        response.Success = false;
                        response.Message = $"Failed to delete Event: {ex.Message}";
                    }
                }
            });

            return response;
        }
        private async Task<string> SaveImageAsync(IFormFile imageFile)
        {
            if (imageFile == null || imageFile.Length == 0)
            {
                throw new ArgumentException("The file is empty or null.", nameof(imageFile));
            }

            // Ensure the uploads directory exists
            var uploadsFolderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads");
            if (!Directory.Exists(uploadsFolderPath))
            {
                Directory.CreateDirectory(uploadsFolderPath);
            }

            // Generate a unique filename for the image to avoid name conflicts
            var fileName = Guid.NewGuid().ToString() + Path.GetExtension(imageFile.FileName);
            var filePath = Path.Combine(uploadsFolderPath, fileName);

            // Save the file
            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                await imageFile.CopyToAsync(fileStream);
            }

            var imageUrl = $"/uploads/{fileName}";
            return imageUrl;
        }
        private static string ConvertToFullUrl(string relativePath)
        {
            if (string.IsNullOrEmpty(relativePath))
                return null;

            var baseUrl = "https://localhost:7139";
            return $"{baseUrl}{relativePath}";
        }

    }
}
