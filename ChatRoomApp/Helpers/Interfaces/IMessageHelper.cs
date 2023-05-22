using ChatRoomApp.Helpers.Dtos;
using ChatRoomApp.Models.Entities;
using ChatRoomApp.Models.ViewModels;

namespace ChatRoomApp.Helpers.Interfaces
{
    public interface IMessageHelper
    {
        Task<List<Message>> MessageListAsync();
        Task<ChatRoomViewModel> ChatRoomData();
        Task AddMessage(MessagePost messagePost);
    }
}
