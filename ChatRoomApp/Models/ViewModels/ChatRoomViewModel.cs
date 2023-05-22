using ChatRoomApp.Models.Entities;

namespace ChatRoomApp.Models.ViewModels
{
    public class ChatRoomViewModel
    {
        public IEnumerable<Message> Messages { get; set; }
        public IEnumerable<User> Users { get; set; }
        public string UserName { get; set; }
    }
}
