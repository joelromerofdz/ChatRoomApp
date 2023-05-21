using ChatRoomApp.Models.Entities;

namespace ChatRoomApp.Helpers.Interfaces
{
    public interface IMessageHelper
    {
        Task<List<Message>> MessageListAsync();
    }
}
