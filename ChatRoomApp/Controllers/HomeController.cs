using ChatRoomApp.Controllers.Base;
using ChatRoomApp.Helpers.Dtos;
using ChatRoomApp.Helpers.Interfaces;
using ChatRoomApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Diagnostics;

namespace ChatRoomApp.Controllers
{
    public class HomeController : BaseChatRoomController
    {
        public readonly IUserHelper _userHelper;
        public readonly IMessageHelper _messageHelper;
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger,
            IUserHelper userHelper,
            IMessageHelper messageHelper)
        {
            _userHelper = userHelper;
            _messageHelper = messageHelper;
            _logger = logger;
        }

        public async Task<IActionResult> Index()
        {
            //var users = await _userHelper.UserListAsync();
            var chatRoom = await _messageHelper.ChatRoomData();
            return View(chatRoom);
        }

        [HttpPost]
        public async Task<IActionResult> AddMessage(MessagePost messagePost)
        {
            if (!ModelState.IsValid)
            {
                var errorMessage = GetModelStateErrors(ModelState);
                return BadRequest(errorMessage);
            }

            await _messageHelper.AddMessage(messagePost);
            return Ok();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public async Task<IActionResult> controllerTest()
        {
            var messages = await _messageHelper.MessageListAsync();
            return View(messages);
        }

        public IActionResult controllerTest2()
        {
            var chatRoom = _messageHelper.ChatRoomData();
            return View(chatRoom);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}