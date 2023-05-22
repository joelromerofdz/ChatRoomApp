using ChatRoomApp.Models.Entities;
using System.ComponentModel.DataAnnotations;

namespace ChatRoomApp.Helpers.Dtos
{
    public class MessagePost
    {
        [Required(ErrorMessage = "Text message is required")]
        public string Content { get; set; }
        [Required(ErrorMessage = "UserId is required")]
        public string UserId { get; set; }
        [Required(ErrorMessage = "ReceiverId is required")]
        public string ReceiverId { get; set; }
    }
}
