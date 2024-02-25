using Lafatkotob.Entities;
using Lafatkotob.Services.EmailService;
using Lafatkotob.Services.TokenService;
using Lafatkotob.ViewModel;
using Lafatkotob.ViewModels;
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

        public async Task<ServiceResponse<AppUser>> RegisterUser(RegisterModel model, string role, IFormFile imageFile)
        {

            var user = new AppUser
            {
                City = model.City,
                DateJoined = DateTime.UtcNow,
                LastLogin = DateTime.UtcNow,
                UserName = model.UserName,
                Email = model.Email,
                Name = model.Name,
                About = model.About,
                DTHDate = model.DTHDate,
            };
            if (imageFile != null)
            {
                var imagePath = await SaveImageAsync(imageFile); 
                user.ProfilePicture = imagePath;
            }

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
        
            var roleResult = await _userManager.AddToRoleAsync(user, role);

            return new ServiceResponse<AppUser>
            {
                Success = true,
                Data = user,
                Message = "User registered successfully. Please check your email to confirm."
            };
        }

        public async Task<ServiceResponse<UpdateUserModel>> UpdateUser(UpdateUserModel model, string userId, IFormFile imageFile)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return new ServiceResponse<UpdateUserModel> { Success = false, Message = "User not found." };
            }
            if (imageFile != null)
            {
                var imagePath = await SaveImageAsync(imageFile); 
                user.ProfilePicture = imagePath; 
            }
            if (model.Email != null && model.Email != user.Email)
            {
                var emailToken = await _userManager.GenerateChangeEmailTokenAsync(user, model.Email);
                var changeEmailResult = await _userManager.ChangeEmailAsync(user, model.Email, emailToken);
                if (!changeEmailResult.Succeeded)
                {
                    return new ServiceResponse<UpdateUserModel>
                    {
                        Success = false,
                        Message = "Failed to update email.",
                        Errors = changeEmailResult.Errors.Select(e => e.Description).ToList()
                    };
                }
                user.EmailConfirmed = false; 
            }


            if (!string.IsNullOrEmpty(model.CurrentPassword) && !string.IsNullOrEmpty(model.NewPassword))
            {
                if (model.NewPassword != model.ConfirmNewPassword)
                {
                    return new ServiceResponse<UpdateUserModel>
                    {
                        Success = false,
                        Message = "New password and confirmation password do not match."
                    };
                }

                var passwordCheck = await _userManager.CheckPasswordAsync(user, model.CurrentPassword);
                if (!passwordCheck)
                {
                    return new ServiceResponse<UpdateUserModel>
                    {
                        Success = false,
                        Message = "Current password is incorrect."
                    };
                }

                var changePasswordResult = await _userManager.ChangePasswordAsync(user, model.CurrentPassword, model.NewPassword);
                if (!changePasswordResult.Succeeded)
                {
                    return new ServiceResponse<UpdateUserModel>
                    {
                        Success = false,
                        Message = "Failed to change password.",
                        Errors = changePasswordResult.Errors.Select(e => e.Description).ToList()
                    };
                }
            }

            user.UserName = model.UserName?? user.UserName;
            user.Name = model.Name ?? user.Name;
            user.City = model.City??user.City;
            user.About = model.About ?? user.About;
     
            var result = await _userManager.UpdateAsync(user);
            if (!result.Succeeded)
            {
                return new ServiceResponse<UpdateUserModel>
                {
                    Success = false,
                    Message = "Failed to update user.",
                    Errors = result.Errors.Select(e => e.Description).ToList()
                };
            }

            return new ServiceResponse<UpdateUserModel> { Success = true ,Data=model};
        }

        public async Task<IEnumerable<AppUserModel>> GetAllUsers()
        {
            var users = await _userManager.Users
                .Select(user => new AppUserModel
                {
                    Id= user.Id,
                    Name = user.Name,
                    Email  = user.Email,
                    City = user.City,
                    DateJoined = user.DateJoined,
                    LastLogin = user.LastLogin,
                    ProfilePicture = user.ProfilePicture,
                    About = user.About,
                    DTHDate = user.DTHDate,
                    HistoryId = user.HistoryId,

                })
                .ToListAsync();
            users.ForEach(user => user.ProfilePicture = ConvertToFullUrl(user.ProfilePicture));

            return users;
        }

        public async Task<AppUserModel> GetUserById(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return null;
            }

            var userModel = new AppUserModel
            {
                Id = user.Id,
                Name = user.Name,
                City = user.City,
                Email = user.Email,
                DateJoined = user.DateJoined,
                LastLogin = user.LastLogin,
                ProfilePicture = user.ProfilePicture,
                About = user.About,
                DTHDate = user.DTHDate,
                HistoryId = user.HistoryId,

            };
            userModel.ProfilePicture = ConvertToFullUrl(userModel.ProfilePicture);

            return userModel;
        }

        public async Task<ServiceResponse<UpdateUserModel>> DeleteUser(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return new ServiceResponse<UpdateUserModel> { Success = false, Message = "User not found." };
            }

            var result = await _userManager.DeleteAsync(user);
            if (!result.Succeeded)
            {
                return new ServiceResponse<UpdateUserModel>
                {
                    Success = false,
                    Message = "Failed to delete user.",
                    Errors = result.Errors.Select(e => e.Description).ToList()
                };
            }
            UpdateUserModel model = new UpdateUserModel
            {
                Name = user.Name,
                Email = user.Email,
                UserName = user.UserName,
                ProfilePictureUrl = user.ProfilePicture,
                About = user.About,
                City = user.City,
                CurrentPassword = null,
                NewPassword = null,
                ConfirmNewPassword = null
            };
            
            

            return new ServiceResponse<UpdateUserModel> { Success = true,Data= model };
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
        private string ConvertToFullUrl(string relativePath)
        {
            if (string.IsNullOrEmpty(relativePath))
                return null;

            // Use your API's base URL here
            var baseUrl = "https://localhost:7139";
            return $"{baseUrl}{relativePath}";
        }



    }
}
