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

        public async Task<List<Message>> MessageListAsync()
        {
            var messages = await _dbcontext.Messages
                .OrderBy(m => m.CreatedDate)
                .Take(50)
                .ToListAsync();

            return messages;
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
                var message = new Message()
                {
                    UserId = messagePost.UserId,
                    //ReceiverId = messagePost.ReceiverId,
                    Content = messagePost.Content
                };

                await _dbcontext.AddAsync(message);
                await _dbcontext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
