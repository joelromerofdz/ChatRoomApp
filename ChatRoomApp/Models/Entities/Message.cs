using ChatRoomApp.Base;

namespace ChatRoomApp.Models.Entities
{
    public class Message : EntityBase
    {
        public string Content { get; set; }
        public string UserId { get; set; }
        public virtual User Sender { get; set; }
    }
}
