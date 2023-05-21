using Microsoft.AspNetCore.Identity;

namespace ChatRoomApp.Models.Entities
{
    public class User : IdentityUser
    {
        public int MessageId { get; set; }
        public virtual IEnumerable<Message> Messages { get; set; }
    }
}
