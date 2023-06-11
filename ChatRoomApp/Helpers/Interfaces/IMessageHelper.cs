using ChatRoomApp.Helpers.Dtos;
using ChatRoomApp.Models.Entities;
using ChatRoomApp.Models.ViewModels;

namespace ChatRoomApp.Helpers.Interfaces
{
    public interface IMessageHelper
    {
        Task<ChatRoomViewModel> ChatRoomData();
        Task AddMessage(MessagePost messagePost);
    }
}
