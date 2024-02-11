﻿using Lafatkotob.Entities;
using Lafatkotob.Services.EmailService;
using Lafatkotob.Services.TokenService;
using Lafatkotob.ViewModel;
using Lafatkotob.ViewModels;
using login.ViewModel;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Lafatkotob.Services.AppUserService
{
    public class UserService : IUserService
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IEmailService _emailService;
        private readonly ITokenSerive _tokenService;

        public UserService(UserManager<AppUser> userManager, IEmailService emailService, ITokenSerive tokenService)
        {
            _userManager = userManager;
            _emailService = emailService;
            _tokenService = tokenService;
        }

        public async Task<ServiceResponse<AppUser>> RegisterUser(RegisterModel model, string baseUrl)
        {
            var user = new AppUser
            {
                City = model.City,
                DateJoined = DateTime.UtcNow,
                LastLogin = DateTime.UtcNow,
                UserName = model.UserName,
                Email = model.Email,
                Name = model.Name,
                ProfilePicture = model.ProfilePictureUrl,
                About = model.About,
                DTHDate = model.DTHDate,
            };

            var result = await _userManager.CreateAsync(user, model.Password);
            if (!result.Succeeded)
            {
                return new ServiceResponse<AppUser>
                {
                    Success = false,
                    Message = "User registration failed.",
                    Errors = result.Errors.Select(e => e.Description).ToList()
                };
            }

            var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            var confirmationLink = $"{baseUrl}/confirm-email?userId={user.Id}&token={Uri.EscapeDataString(token)}";
            var emailBody = $"Please confirm your account by <a href='{confirmationLink}'>clicking here</a>.";

            await _emailService.SendEmailAsync(user.Email, "Confirm Your Email", emailBody);

            return new ServiceResponse<AppUser>
            {
                Success = true,
                Data = user,
                Message = "User registered successfully. Please check your email to confirm."
            };
        }

        public async Task<ServiceResponse<bool>> UpdateUser(UpdateUserModel model, string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return new ServiceResponse<bool> { Success = false, Message = "User not found." };
            }

            // Map the update model to the user entity
            user.Email = model.Email;
            user.UserName = model.UserName;
            user.Name = model.Name;
            user.City = model.City;
            user.ProfilePicture = model.ProfilePictureUrl;
            user.About = model.About;
     
            var result = await _userManager.UpdateAsync(user);
            if (!result.Succeeded)
            {
                return new ServiceResponse<bool>
                {
                    Success = false,
                    Message = "Failed to update user.",
                    Errors = result.Errors.Select(e => e.Description).ToList()
                };
            }

            return new ServiceResponse<bool> { Success = true };
        }

        public async Task<IEnumerable<AppUserModel>> GetAllUsers()
        {
            var users = await _userManager.Users
                .Select(user => new AppUserModel
                {
                    City = user.City,
                    DateJoined = user.DateJoined,
                    LastLogin = user.LastLogin,
                    ProfilePicture = user.ProfilePicture,
                    About = user.About,
                    DTHDate = user.DTHDate,
                    HistoryId = user.HistoryId,
                })
                .ToListAsync();

            return users;
        }

        public async Task<AppUserModel> GetUserById(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return null;
            }

            return new AppUserModel
            {
                Name = user.Name,
                City = user.City,
                DateJoined = user.DateJoined,
                LastLogin = user.LastLogin,
                ProfilePicture = user.ProfilePicture,
                About = user.About,
                DTHDate = user.DTHDate,
                HistoryId = user.HistoryId,

            };
        }

        public async Task<ServiceResponse<bool>> DeleteUser(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return new ServiceResponse<bool> { Success = false, Message = "User not found." };
            }

            var result = await _userManager.DeleteAsync(user);
            if (!result.Succeeded)
            {
                return new ServiceResponse<bool>
                {
                    Success = false,
                    Message = "Failed to delete user.",
                    Errors = result.Errors.Select(e => e.Description).ToList()
                };
            }

            return new ServiceResponse<bool> { Success = true };
        }

        public async Task<LoginResultModel> LoginUser(LoginModel model)
        {
            var user = await _userManager.FindByNameAsync(model.UserName);
            if (user == null || !await _userManager.CheckPasswordAsync(user, model.Password))
            {
                return new LoginResultModel { Success = false, ErrorMessage = "Invalid username or password." };
            }

            var roles = await _userManager.GetRolesAsync(user);
            var token = _tokenService.GenerateJwtToken(user.UserName, user.Id, roles.ToList());

            return new LoginResultModel
            {
                Success = true,
                Token = token
            };
        }

        public async Task<ServiceResponse<bool>> ConfirmEmail(string userId, string token)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return new ServiceResponse<bool> { Success = false, Message = "User not found." };
            }

            var result = await _userManager.ConfirmEmailAsync(user, token);
            if (!result.Succeeded)
            {
                return new ServiceResponse<bool>
                {
                    Success = false,
                    Message = "Email confirmation failed.",
                    Errors = result.Errors.Select(e => e.Description).ToList()
                };
            }

            return new ServiceResponse<bool> { Success = true, Data = true };
        }
    }
}