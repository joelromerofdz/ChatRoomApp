using ChatRoomApp.Helpers;
using ChatRoomApp.Helpers.Interfaces;
using ChatRoomApp.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace ChatRoomApp.Controllers
{
    public class HomeController : Controller
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
            var users = await _userHelper.UserListAsync();
            return View(users);
        }

        public IActionResult Privacy()
        {
            return View();
        }
        public IActionResult controllerTest()
        {
            var messages = _messageHelper.MessageListAsync();
            return View(messages);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}