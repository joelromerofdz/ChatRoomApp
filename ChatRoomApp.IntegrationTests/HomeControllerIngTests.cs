using ChatRoomApp.Data;
using ChatRoomApp.Helpers.Dtos;
using ChatRoomApp.Models.ViewModels;
using ChatRoomApp.TestUtilities.IntegrationTests;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using NUnit.Framework.Internal;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;

namespace ChatRoomApp.IntegrationTests
{
    public class HomeControllerIngTests
    {
        private HttpClient _httpClient;
        private TestAuthHandler _authHandler;

        [SetUp]
        public void Setup()
        {
            //in-memory db
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase("DatabaseTest")
                .Options;

            var dbContext = new AppDbContext(options);

            var appFactory = new WebApplicationFactory<Program>()
                .WithWebHostBuilder(builder =>
                {
                    builder.ConfigureServices(services =>
                    {
                        services.RemoveAll(typeof(AppDbContext));
                        services.AddSingleton<AppDbContext>(dbContext);
                        //Authentication
                        services.AddAuthentication("Test")
                        .AddScheme<AuthenticationSchemeOptions, TestAuthHandler>("Test", options => { });
                    });
                });

            _httpClient = appFactory.CreateDefaultClient();
            //Authentication
            //_authHandler = appFactory.Services.GetRequiredService<TestAuthHandler>();
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Test");
        }

        [Test]
        public async Task AddMessage_WhenSendAMessage_ReturnStatusCodeOk200()
        {
            var message = new MessageRequest
            {
                Content = "Hi 4-1",
                UserId = UserSettings.UserId
            };

            var jsonOption = new JsonSerializerOptions()
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };

            var response = await _httpClient
                .PostAsJsonAsync("/Home/AddMessage/", message, jsonOption);

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
        }

        [TestCase(null, UserSettings.UserId)]
        [TestCase("Hello Test!", null)]
        [TestCase(null, null)]
        [Test]
        public async Task AddMessage_WhenModelStateIsNotValid_ReturnABadRequest400(string content, string userId)
        {
            #region Arrage
            MessageRequest messagePost = new MessageRequest()
            {
                Content = content,
                UserId = userId
            };
            #endregion

            #region Act
            var response = await _httpClient.PostAsJsonAsync("/home/addmessage", messagePost);
            var contentResponse = response.Content.ReadAsStringAsync();
            #endregion

            #region Assert
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
            #endregion
        }

        [TestCase("/stock=aapl.us", UserSettings.UserId)]
        [Test]
        public async Task AddMessage_WhenStockCodeIsInvoked_ReturnBotMessage(string content, string userId)
        {
            #region Arrage
            MessageRequest message = new MessageRequest()
            {
                Content = content,
                UserId = userId
            };
            #endregion

            #region Act
            var reponse = await _httpClient.PostAsJsonAsync("/home/addmessage", message);
            var contentResponse = reponse.Content.ReadAsStringAsync();
            #endregion

            #region Assert
            Assert.That(reponse.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            Assert.That(contentResponse.Result, Does.Contain("aapl.us"));
            #endregion
        }

        [Test]
        [Ignore("Reason for ignoring this test")]
        public async Task Test()
        {
            //DbContextOptionsBuilder<AppDbContext> optionsBuilder = new();
            //optionsBuilder.UseInMemoryDatabase(MethodBase.GetCurrentMethod().Name);

            //Task<IActionResult> result;

            //var repository = _serviceProvider.GetService<IChatRoomAppRepository>();
            //var logger = _serviceProvider.GetService<ILogger<HomeController>>();
            //var chatBotStock = _serviceProvider.GetService<ChatBotStock>();
            //var controller = new HomeController(repository, logger, chatBotStock);

            /*using (AppDbContext ctx = new(optionsBuilder.Options))
            {*/
            var jsonOption = new JsonSerializerOptions() 
            { 
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase 
            };

            var result = new MessageRequest
                {
                    Content = "Hi 100",
                    UserId = "a74dfb84-dfb9-4194-bab5-d404be6e833f"
                };
            /*}*/
            //var k = JsonConvert.SerializeObject(result);
            //var content = new StringContent(JsonConvert.SerializeObject(result),
            //                    Encoding.UTF8,
            //                    "application/json");

            var response = await _httpClient
                .PostAsJsonAsync("/Home/AddMessage/", result, jsonOption);
            //response.EnsureSuccessStatusCode();

            var responseContent = await response.Content.ReadAsStringAsync();

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
        }

        
        [Test]
        [Ignore("Reason for ignoring this test")]
        public async Task Test1()
        {
            #region Act
            var response = await _httpClient.GetAsync("/home/welcome?id=1");
            var stringResult = await response.Content.ReadAsStringAsync();
            #endregion

            #region Assert
            Assert.That(stringResult, Is.EqualTo("hi mundo"));
            #endregion
        }
    }
}