using ChatRoomApp.Data;
using ChatRoomApp.Helpers;
using ChatRoomApp.Helpers.Interfaces;
using ChatRoomApp.Models.Entities;
using ChatRoomApp.Models.ViewModels;
using ChatRoomApp.TestUtilities.Tests;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Moq;
using NUnit.Framework;

namespace ChatRoomApp.UnitTests.HelpersUnitTests
{
    [TestFixture]
    public class MessageHelperTests : BasedMockedUnitTest
    {
        private Mock<AppDbContext> _dbcontext = new Mock<AppDbContext>();
        private Mock<IUserHelper> _userHelper = new Mock<IUserHelper>();
        private MessageHelper _sut;

        [SetUp]
        public void SetUp()
        {
            _sut = new MessageHelper(_dbcontext.Object, _userHelper.Object);
        }

        [Test]
        public async Task ChatRoomData_WhenCalled_RerturnChatRoomInformation()
        {
            #region Arrage
            var users = GetFakeUsers(2);
            var messages = GetFakeMessages(50, users[0].Id, users[1].Id);
            var mockDbSet = CreateMockDbSet<Message>(messages.AsQueryable());
            
            var userInfo = new UserInfo()
            {
                UserId = users[1].Id,
                UserName = users[1].UserName
            };

            _dbcontext.Setup(m => m.Messages)
                .Returns(mockDbSet.Object);

            _userHelper.Setup(m => m.UserListAsync())
                .ReturnsAsync(users);

            _userHelper.Setup(m => m.GetUserId())
               .Returns(userInfo);

            //IUserHelper userHelper = _userHelper.Object;
            var expected = new ChatRoomViewModel
            {
                Messages = messages,
                Users = users, //await userHelper.UserListAsync(),
                UserName = userInfo.UserName,
                UserId = userInfo.UserId
            };
            #endregion

            #region Act
            var result = await _sut.ChatRoomData();
            #endregion

            #region Assert
            Assert.That(result.Messages.Count, Is.EqualTo(50));
            Assert.That(result.Messages, Is.EquivalentTo(expected.Messages));
            Assert.That(result.Users, Is.EquivalentTo(expected.Users));
            Assert.That(result.UserName, Is.EqualTo(result.UserName));
            Assert.That(result.UserId, Is.EqualTo(result.UserId));
            #endregion
        }

        [Test]
        public async Task ChatRoomData_WhenDoesNotExistMessages_RerturnAnEmptyListMessage()
        {
            #region Arrage
            var users = GetFakeUsers(2);
            var messages = new List<Message>();
            var mockDbSet = CreateMockDbSet<Message>(messages.AsQueryable());

            var userInfo = new UserInfo()
            {
                UserId = users[1].Id,
                UserName = users[1].UserName
            };

            _dbcontext.Setup(m => m.Messages)
                .Returns(mockDbSet.Object);

            _userHelper.Setup(m => m.UserListAsync())
                .ReturnsAsync(users);

            _userHelper.Setup(m => m.GetUserId())
               .Returns(userInfo);

            var expected = new ChatRoomViewModel
            {
                Messages = new List<Message>(),
                Users = users,
                UserName = userInfo.UserName,
                UserId = userInfo.UserId
            };
            #endregion

            #region Act
            var result = await _sut.ChatRoomData();
            #endregion

            #region Assert
            Assert.That(result.Messages.Count, Is.EqualTo(0));
            Assert.That(result.Users.Count, Is.EqualTo(2));
            Assert.That(result.UserName, Is.EqualTo(result.UserName));
            Assert.That(result.UserId, Is.EqualTo(result.UserId));
            #endregion
        }

        private List<Message> GetFakeMessages(int total, string user1, string user2)
        {
            var messages = new List<Message>();
           
            for (int i = 1; i <= total; i++)
            {
                var message = new Message()
                {
                    Id = i,
                    Content = $"Message {Guid.NewGuid().ToString()}",
                    UserId = i % 2 == 0 ? user1 : user2
                };

                messages.Add(message);
            }
            return messages;
        }

        private List<User> GetFakeUsers(int total)
        {
            var users = new List<User>();

            for (int i = 1; i <= total; i++)
            {
                var user = new User
                {
                    Id = Guid.NewGuid().ToString(),
                    UserName = $"User {Guid.NewGuid().ToString()}",
                    NormalizedUserName = "",
                    Email = "",
                    NormalizedEmail = "",
                    EmailConfirmed = false,
                    PasswordHash = Guid.NewGuid().ToString(),
                    SecurityStamp = "",
                    ConcurrencyStamp = Guid.NewGuid().ToString(),
                    PhoneNumber = "",
                    PhoneNumberConfirmed = false,
                    TwoFactorEnabled = false,
                    LockoutEnd = null,
                    LockoutEnabled = false,
                    AccessFailedCount = 0
                };

                users.Add(user);
            }
            return users;
        }
    }
}
