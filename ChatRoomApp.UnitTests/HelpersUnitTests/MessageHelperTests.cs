using ChatRoomApp.Data;
using ChatRoomApp.Helpers;
using ChatRoomApp.Helpers.Dtos;
using ChatRoomApp.Helpers.Interfaces;
using ChatRoomApp.Models.Entities;
using ChatRoomApp.Models.ViewModels;
using ChatRoomApp.TestUtilities.Tests;
using Moq;

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

        [TearDown]
        public void TearDown()
        {
            _dbcontext.Reset();
            _userHelper.Reset();
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
            Assert.That(result.UserName, Is.EqualTo(expected.UserName));
            Assert.That(result.UserId, Is.EqualTo(expected.UserId));
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

        [Test]
        public async Task AddMessage_WhenPassTheMessageValue_SaveTheMessageValueInDataBase()
        {
            #region Arragen
            var message = new MessagePost()
            {
                UserId = Guid.NewGuid().ToString(),
                Content = "Hi!"
            };

            _dbcontext.Setup(m => m.SaveChangesAsync(It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult(0));
            #endregion

            #region Act
            await _sut.AddMessage(message);
            #endregion

            #region Assert
            _dbcontext.Verify(m => m.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
            #endregion
        }

        [Test]
        public void AddMessage_WhenFailed_ThrowsException()
        {
            #region Arragen
            var message = new MessagePost();

            var expected = "User Id and Content cannot be empty.";

            _dbcontext.Setup(m => m.AddAsync(It.IsAny<Message>(), default(CancellationToken)))
                .ThrowsAsync(new Exception(expected));

            _dbcontext.Setup(m => m.SaveChangesAsync(default(CancellationToken)))
                .ThrowsAsync(new Exception(expected));
            #endregion

            #region Act
            var exception = Assert.ThrowsAsync<Exception>(async () => await _sut.AddMessage(message));
            #endregion

            #region Assert
            Assert.That(exception.Message, Is.EqualTo(expected));
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
