using Lafatkotob.ViewModel;
using Lafatkotob.Entities;
using Lafatkotob.ViewModels;
using Microsoft.AspNetCore.Identity;
using login.ViewModel;

namespace Lafatkotob.Services.AppUserService
{
    public interface IUserService
    {
        Task<ServiceResponse<AppUser>> RegisterUser(RegisterModel model,string rule);
        Task<ServiceResponse<UpdateUserModel>> UpdateUser(UpdateUserModel model, string userId);
        Task<IEnumerable<AppUserModel>> GetAllUsers();
        Task<AppUserModel> GetUserById(string userId);
        Task<ServiceResponse<UpdateUserModel>> DeleteUser(string userId);
        Task<LoginResultModel> LoginUser(LoginModel model);

    }
}
