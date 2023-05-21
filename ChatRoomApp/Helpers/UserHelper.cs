using ChatRoomApp.Controllers;
using ChatRoomApp.Data;
using ChatRoomApp.Helpers.Interfaces;
using ChatRoomApp.Models.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace ChatRoomApp.Helpers
{
    public class UserHelper : IUserHelper
    {
        public readonly AppDbContext _dbcontext;
        public readonly UserManager<User> _userManager;

        public UserHelper(AppDbContext dbcontext, UserManager<User> userManager)
        {
            _dbcontext = dbcontext;
            _userManager = userManager;
        }

        public async Task<List<User>> UserListAsync()
        {
            var users = await _dbcontext.Users
                .ToListAsync();

            /*if (!users.Any())
            {
                return new List<User>();
            }*/

            return users;
        }
    }
}
