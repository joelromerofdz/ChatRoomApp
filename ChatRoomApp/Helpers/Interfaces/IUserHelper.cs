using ChatRoomApp.Models.Entities;

namespace ChatRoomApp.Helpers.Interfaces
{
    public interface IUserHelper
    {
        Task<List<User>> UserListAsync();
    }
}
