using ChatBot;
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
    //[AllowAnonymous]
    public class HomeController : BaseChatRoomController
    {
        private readonly IChatRoomAppRepository _repository;
        private readonly ChatBotStock _chatBotStock;
        private readonly ILogger<HomeController> _logger;

        public HomeController(IChatRoomAppRepository repository,
            ILogger<HomeController> logger,
            ChatBotStock chatBotStock)
        {
            _repository = repository;
            _logger = logger;
            _chatBotStock = chatBotStock;
        }

        public async Task<IActionResult> Index()
        {
            var chatRoom = await _repository.MessageHelper.ChatRoomData();
            return View(chatRoom);
        }

        [HttpPost]
        public async Task<IActionResult> AddMessage([FromBody] MessageRequest messagePost)
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

            await _repository.MessageHelper.AddMessage(messagePost);
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

        [HttpGet]
        public string Welcome(int id)
        {
            return "hola mundo";
        }
    }
}