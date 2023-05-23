using ChatRoomApp.API.External;
using ChatRoomApp.Controllers.Base;
using ChatRoomApp.Helpers.Dtos;
using ChatRoomApp.Helpers.Interfaces;
using ChatRoomApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Diagnostics;

namespace ChatRoomApp.Controllers
{
    [Authorize]
    public class HomeController : BaseChatRoomController
    {
        public readonly IUserHelper _userHelper;
        public readonly IMessageHelper _messageHelper;
        private readonly ChatBotStock _chatBotStock;
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger,
            IUserHelper userHelper,
            IMessageHelper messageHelper,
            ChatBotStock chatBotStock)
        {
            _userHelper = userHelper;
            _messageHelper = messageHelper;
            _logger = logger;
            _chatBotStock = chatBotStock;
        }

        public async Task<IActionResult> Index()
        {
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

            if (_chatBotStock.IsStockCodeInvoked(messagePost.Content))
            {
                var botMessage = await _chatBotStock.GetBotStock(messagePost.Content);
                return Ok(JsonConvert.SerializeObject(new { BotMessage = botMessage }));
            }

            await _messageHelper.AddMessage(messagePost);
            return Ok(JsonConvert.SerializeObject(new { BotMessage = "" }));
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}