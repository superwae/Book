using Lafatkotob.ViewModel;
using Lafatkotob.Entities;
using Lafatkotob.ViewModels;
using Microsoft.AspNetCore.Identity;
using login.ViewModel;

namespace Lafatkotob.Services.AppUserService
{
    public interface IUserService
    {
        Task<RegisterModel> RegisterUser(RegisterModel model,string role);
        Task UpdateUser(UpdateUserModel model, string userId);
        Task<IEnumerable<AppUserModel>> GetAllUsers(); 
        Task<AppUserModel> GetUserById(string userId);
        Task<AppUserModel> DeleteUser(string userId);


    }
}
