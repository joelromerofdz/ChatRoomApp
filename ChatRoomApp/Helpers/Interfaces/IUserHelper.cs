using ChatRoomApp.Helpers.Dtos;
using ChatRoomApp.Models.Entities;
using ChatRoomApp.Models.ViewModels;
using Microsoft.AspNetCore.Identity;

namespace ChatRoomApp.Helpers.Interfaces
{
    public interface IUserHelper
    {
        Task<List<User>> UserListAsync();
        UserInfo GetUserId();
        Task<IdentityResult> AddUserSync(RegisterViewModel register);
        Task<SignInResult> LogInUser(LoginPost login, bool lockoutOnFailure);
        bool IsUserAuthenticated();
        Task LogOut();
    }
}
