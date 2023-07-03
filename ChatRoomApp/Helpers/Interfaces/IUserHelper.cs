using ChatRoomApp.Helpers.Dtos;
using ChatRoomApp.Models.Entities;
using ChatRoomApp.Models.ViewModels;
using Microsoft.AspNetCore.Identity;

namespace ChatRoomApp.Helpers.Interfaces
{
    public interface IUserHelper
    {
        Task<List<User>> UserListAsync();
        UserInfoResponse GetUserId();
        Task<IdentityResult> AddUserSync(RegisterViewModel register);
        Task<SignInResult> LogInUser(LoginRequest login, bool lockoutOnFailure);
        bool IsUserAuthenticated();
        Task LogOut();
    }
}
