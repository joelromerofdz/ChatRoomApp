using ChatRoomApp.Data;
using ChatRoomApp.Helpers.Dtos;
using ChatRoomApp.Helpers.Interfaces;
using ChatRoomApp.Models.Entities;
using ChatRoomApp.Models.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace ChatRoomApp.Helpers
{
    public class MessageHelper : IMessageHelper
    {
        public readonly AppDbContext _dbcontext;
        public readonly IUserHelper _userHelper;

        public MessageHelper(AppDbContext dbcontext,
            IUserHelper userHelper)
        {
            _dbcontext = dbcontext;
            _userHelper = userHelper;
        }

        public async Task<ChatRoomViewModel> ChatRoomData()
        {
            var messages = await MessageListAsync();
            var users = await _userHelper.UserListAsync();

            var chatRoomViewModel = new ChatRoomViewModel
            {
                Messages = messages,
                Users = users,
                UserName = _userHelper.GetUserId().UserName,
                UserId = _userHelper.GetUserId().UserId 
            };

            return chatRoomViewModel;
        }

        public async Task AddMessage(MessagePost messagePost)
        {
            try
            {
                var message = MapMessagePostToMessage(messagePost);

                await _dbcontext.AddAsync(message);
                await _dbcontext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        private async Task<List<Message>> MessageListAsync()
        {
            var messages = new List<Message>();

            var lastMessages = await _dbcontext.Messages
                .OrderByDescending(m => m.CreatedDate)
                .Take(50)
                .ToListAsync() ?? new List<Message>();

            if (lastMessages.Any())
            {
                messages = lastMessages
               .OrderBy(m => m.CreatedDate)
               .ToList();
            }

            return messages;
        }

        private Message MapMessagePostToMessage(MessagePost messagePost)
        {
            if (string.IsNullOrWhiteSpace(messagePost.UserId) || 
                string.IsNullOrWhiteSpace(messagePost.Content))
            {
                throw new Exception("User Id and Content cannot be empty.");
            }

            var message = new Message()
            {
                UserId = messagePost.UserId,
                //ReceiverId = messagePost.ReceiverId,
                Content = messagePost.Content
            };

            return message;
        }

    }
}
