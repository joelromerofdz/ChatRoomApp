using ChatRoomApp.Data;
using ChatRoomApp.Helpers.Interfaces;
using ChatRoomApp.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace ChatRoomApp.Helpers
{
    public class MessageHelper : IMessageHelper
    {
        public readonly AppDbContext _dbcontext;

        public MessageHelper(AppDbContext dbcontext)
        {
            _dbcontext = dbcontext;
        }

        public async Task<List<Message>> MessageListAsync()
        {
            var messages = await _dbcontext.Messages.ToListAsync();

            return messages;
        }
    }
}
