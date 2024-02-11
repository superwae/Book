using Azure.Core;
using Lafatkotob.Entities;
using Lafatkotob.Services.AppUserService;
using Lafatkotob.Services.EmailService;
using Lafatkotob.ViewModel;
using Lafatkotob.ViewModels;
using login.ViewModel;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;

namespace Lafatkotob.Services.AppUserService
{
    public class UserService : IUserService
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly IEmailService _emailService;
        public UserService(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, IEmailService emailService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _emailService = emailService;
        }

        public async Task<AppUserModel> DeleteUser(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null) return null;

            {

                var result = await _userManager.DeleteAsync(user);

            }
            return new AppUserModel
            {
                City = user.City,
                DateJoined = user.DateJoined,
                LastLogin = user.LastLogin,
                ProfilePicture = user.ProfilePicture,
                About = user.About,
                DTHDate = user.DTHDate,
                HistoryId = user.HistoryId,
            };

        }



        public async Task<IEnumerable<AppUserModel>> GetAllUsers()
        {
            return await _userManager.Users
                .Select(up => new AppUserModel
                {
                    City = up.City,
                    DateJoined = up.DateJoined,
                    LastLogin = up.LastLogin,
                    ProfilePicture = up.ProfilePicture,
                    About = up.About,
                    DTHDate = up.DTHDate,
                    HistoryId = up.HistoryId,
                    
                })
                    .ToListAsync();
        }



        public async Task<AppUserModel> GetUserById(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);

            var up = new AppUserModel
            {
                City = user.City,
                DateJoined = user.DateJoined,
                LastLogin = user.LastLogin,
                ProfilePicture = user.ProfilePicture,
                About = user.About,
                DTHDate = user.DTHDate,
                HistoryId = user.HistoryId,

            };
            return up;



        }

   

        public async Task<RegisterModel> RegisterUser(RegisterModel model, string role)
        {
            var user = new AppUser
            {
                City=model.City,
                DateJoined = DateTime.Now,
                LastLogin = DateTime.Now,
                UserName = model.UserName,
                Email = model.Email,
                Name = model.Name,
                ProfilePicture =model.ProfilePictureUrl,
                About= model.About,
                DTHDate = model.DTHDate,
                
            };

            var result = await _userManager.CreateAsync(user, model.Password);
            if (result.Succeeded)
            {
                // Assign Roles
                var roleResult = await _userManager.AddToRoleAsync(user, role);
                // Check roleResult for success
                return model;
            }
            else
            {
                return null;
            }
        }


        public async Task UpdateUser(UpdateUserModel model, string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user != null)
            {
                user.Email = model.Email;
                user.UserName = model.UserName;
                user.Name = model.Name;
                var updateResult = await _userManager.UpdateAsync(user);

                // Handle password change
                if (!string.IsNullOrEmpty(model.NewPassword))
                {
                    var passwordChangeResult = await _userManager.ChangePasswordAsync(user, model.CurrentPassword, model.NewPassword);
                }
            }
        }

    }
}
