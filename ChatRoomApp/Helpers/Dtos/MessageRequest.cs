﻿using System.ComponentModel.DataAnnotations;

namespace ChatRoomApp.Helpers.Dtos
{
    public class MessageRequest
    {
        [Required(ErrorMessage = "Text message is required")]
        public string Content { get; set; }
        [Required(ErrorMessage = "UserId is required")]
        public string UserId { get; set; }
        //[Required(ErrorMessage = "ReceiverId is required")]
        //public string ReceiverId { get; set; }
    }
}
