using Microsoft.AspNetCore.Identity;

namespace ChatRoomApp.Models.Entities
{
    public class User : IdentityUser
    {
        public virtual IEnumerable<Message> Messages { get; set; }
    }
}
