using ChatRoomApp.Data;
using ChatRoomApp.Helpers.Interfaces;
using ChatRoomApp.Models.Entities;
using Microsoft.AspNetCore.Identity;

namespace ChatRoomApp.Helpers
{
    public class ChatRoomAppRepository : IChatRoomAppRepository
    {
        private readonly AppDbContext _dbcontext;
        private UserHelper? _userHelper;
        private MessageHelper? _messageHelper;
        private IUserHelper _iUserHelper;
        private UserManager<User> _userManager;
        private SignInManager<User> _signInManager;
        private IHttpContextAccessor _httpContextAccessor;

        public ChatRoomAppRepository(AppDbContext dbcontext,
            UserManager<User> userManager,
            SignInManager<User> signInManager,
            IHttpContextAccessor httpContextAccessor,
            IUserHelper iUserHelper)
        {
            _dbcontext = dbcontext;
            _userManager = userManager;
            _signInManager = signInManager;
            _httpContextAccessor = httpContextAccessor;
            _iUserHelper = iUserHelper;
        }

        public IMessageHelper MessageHelper
        {
            get
            {
                return _messageHelper ??= 
                    new MessageHelper(_dbcontext, _iUserHelper);
            }
        }

        public IUserHelper UserHelper
        {
            get
            {
                return _userHelper ??= 
                    new UserHelper(_dbcontext,
                    _userManager,
                    _signInManager,
                    _httpContextAccessor);
            }
        }
    }
}
