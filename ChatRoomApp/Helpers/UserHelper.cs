using ChatRoomApp.Data;
using ChatRoomApp.Helpers.Dtos;
using ChatRoomApp.Helpers.Interfaces;
using ChatRoomApp.Models.Entities;
using ChatRoomApp.Models.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace ChatRoomApp.Helpers
{
    public class UserHelper : IUserHelper
    {
        public readonly AppDbContext _dbcontext;
        public readonly UserManager<User> _userManager;
        public readonly SignInManager<User> _signInManager;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ClaimsPrincipal _userLogIn;

        public UserHelper(AppDbContext dbcontext,
            UserManager<User> userManager,
            SignInManager<User> signInManager,
            IHttpContextAccessor httpContextAccessor)
        {
            _dbcontext = dbcontext;
            _userManager = userManager;
            _signInManager = signInManager;
            _httpContextAccessor = httpContextAccessor;
            _userLogIn = _httpContextAccessor.HttpContext.User;
        }

        public async Task<List<User>> UserListAsync()
        {
            var users = await _dbcontext.Users
                .ToListAsync();

            return users;
        }

        public UserInfoResponse GetUserId()
        {
            var userInfo = new UserInfoResponse()
            {
                UserId = _userManager.GetUserId(_userLogIn),
                UserName = _userLogIn.Identity.Name
            };

            return userInfo;
        }

        public async Task<IdentityResult> AddUserSync(RegisterViewModel register)
        {
            var user = new User()
            {
                UserName = register.UserName
            };

            var result = await _userManager.CreateAsync(user, register.Password);
            if (result.Succeeded)
            {
                await _signInManager.SignInAsync(user, false);
            }

            return result;
        }

        public async Task<SignInResult> LogInUser(LoginRequest login, bool lockoutOnFailure)
        {
            var signInResult = await _signInManager.PasswordSignInAsync(login.UserName, login.Password, login.RememberMe, lockoutOnFailure);
            return signInResult;
        }

        public bool IsUserAuthenticated()
        {
            bool isAuthenticated = _userLogIn.Identity.IsAuthenticated;
            return isAuthenticated;
        }

        public async Task LogOut()
        {
            await _signInManager.SignOutAsync();
        }
    }
}
