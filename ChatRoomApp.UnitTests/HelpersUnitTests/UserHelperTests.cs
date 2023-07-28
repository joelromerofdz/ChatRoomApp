using ChatRoomApp.Data;
using ChatRoomApp.Helpers;
using ChatRoomApp.Helpers.Dtos;
using ChatRoomApp.Models.Entities;
using ChatRoomApp.Models.ViewModels;
using ChatRoomApp.TestUtilities.Tests;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using System.Security.Claims;

namespace ChatRoomApp.UnitTests.HelpersUnitTests
{
    [TestFixture]
    public class UserHelperTests : BasedMockedUnitTest
    {
        private Mock<AppDbContext> _dbcontext = new Mock<AppDbContext>();
        private Mock<UserManager<User>> _userManager;
        private Mock<SignInManager<User>> _signInManager;
        private Mock<IHttpContextAccessor> _httpContextAccessor;
        public ClaimsPrincipal _userLogIn;
        private UserHelper _sut;

        [SetUp]
        public void SetUp()
        {
            var userStore = new Mock<IUserStore<User>>();
            _userManager = new Mock<UserManager<User>>(
                userStore.Object,
                    new Mock<IOptions<IdentityOptions>>().Object,
                    new Mock<IPasswordHasher<User>>().Object,
                    new IUserValidator<User>[0],
                    new IPasswordValidator<User>[0],
                    new Mock<ILookupNormalizer>().Object,
                    new Mock<IdentityErrorDescriber>().Object,
                    new Mock<IServiceProvider>().Object,
                    new Mock<ILogger<UserManager<User>>>().Object);

            var contextAccessor = new Mock<IHttpContextAccessor>();
            var claimsFactory = new Mock<IUserClaimsPrincipalFactory<User>>();

            _signInManager = new Mock<SignInManager<User>>(
                _userManager.Object,
                contextAccessor.Object,
                claimsFactory.Object,
                null,
                null,
                null,
                null);

            _httpContextAccessor = new Mock<IHttpContextAccessor>();
            //user authentication
            _userLogIn = new ClaimsPrincipal();

            _httpContextAccessor.Setup(m => m.HttpContext.User)
                 .Returns(_userLogIn);

            _sut = new UserHelper(_dbcontext.Object,
                _userManager.Object,
                _signInManager.Object,
                _httpContextAccessor.Object);
        }

        [TearDown]
        public void TearDown()
        {
            _dbcontext.Reset();
            _userManager.Reset();
            _signInManager.Reset();
            _httpContextAccessor.Reset();
        }

        [Test]
        public async Task UserListAsync_WhenCalled_ReturnListOfUser()
        {
            #region Arrage
            int total = 3;
            var users = GetFakeUserList(total);

            var mockDbSet = CreateMockDbSet<User>(users.AsQueryable());

            _dbcontext.Setup(m => m.Users)
                .Returns(mockDbSet.Object);
            #endregion

            #region Act
            var result = await _sut.UserListAsync();
            #endregion

            #region Assert
            Assert.That(result.Count, Is.EqualTo(total));
            #endregion
        }

        [Test]
        public void GetUserId_WhenCalled_ReturnUserInfo()
        {
            #region Arrage
            UserAuthenticated("12345", "testuser", "mockAuthenticationType");

            var userName = _userLogIn.Identity.Name;
            var userId = "12345";

            _userManager.Setup(m => m.GetUserId(_userLogIn))
                 .Returns(userId);

            var expected = new UserInfoResponse()
            {
                UserId = userId,
                UserName = userName
            };
            #endregion

            #region Act
            var result = _sut.GetUserId();
            #endregion

            #region Assert
            Assert.That(result.UserId, Is.EqualTo(expected.UserId));
            Assert.That(result.UserName, Is.EqualTo(expected.UserName));
            #endregion
        }

        [Test]
        public async Task AddUserSync_WhenUserIsCreatedSuccefully_ThenCreateUserAndSignInTheSpecificUser()
        {
            #region Arrage
            var register = new RegisterViewModel()
            {
                UserName = "TestUser100",
                Password = "T3@109User",
                ComfirmPassword = "T3@109User"
            };

            var user = new User()
            {
                UserName = register.UserName
            };

            var expectedResult = IdentityResult.Success;

            _userManager.Setup(m => m.CreateAsync(It.IsAny<User>(), register.Password))
                .ReturnsAsync(expectedResult);

            _signInManager.Setup(m => m.SignInAsync(user, false, null))
                .Returns(Task.CompletedTask);
            #endregion

            #region Act
            var result = await _sut.AddUserSync(register);
            #endregion

            #region Assert
            Assert.That(result.Succeeded, Is.EqualTo(expectedResult.Succeeded));
            #endregion
        }

        [Test]
        public async Task AddUserSync_WhenUserIsNotCreatedSuccefully_ReturnAnIdentityResult()
        {
            #region Arrage
            var register = new RegisterViewModel()
            {
                UserName = null,
                Password = "T3@109User",
                ComfirmPassword = "T3@109User"
            };

            var user = new User()
            {
                UserName = register.UserName
            };

            var error = new IdentityError()
            {
                Code = "4023",
                Description = "UserName cannot be null",
            };

            var expectedResult = IdentityResult.Failed(error);

            _userManager.Setup(m => m.CreateAsync(It.IsAny<User>(), register.Password))
                .ReturnsAsync(expectedResult);
            #endregion

            #region Act
            var result = await _sut.AddUserSync(register);
            #endregion

            #region Assert
            Assert.That(result, Is.TypeOf<IdentityResult>());
            #endregion
        }


        [Test]
        [TestCase("TestUser", "T@user100", "Succeeded")]
        [TestCase("TestUser", "Tuser0", "Failed")]
        public async Task LogInUser_WhenUserLogsIn_ReturnSignInResult(string UserName, string Password, string signInResult)
        {
            #region Arrage
            var login = new LoginRequest()
            {
                UserName = UserName,
                Password = Password,
                RememberMe = false
            };

            var lockoutOnFailure = false;
            var expectedResult = TestSignInResult.SignStatus(signInResult);

            _signInManager.Setup(m => m.PasswordSignInAsync(login.UserName,
                login.Password,
                login.RememberMe,
                lockoutOnFailure))
                .ReturnsAsync(expectedResult);
            #endregion

            #region Act
            var result = await _sut.LogInUser(login, lockoutOnFailure);
            #endregion

            #region Assert
            Assert.That(result, Is.EqualTo(expectedResult));
            #endregion
        }

        [Test]
        public void IsUserAuthenticated_WhenUserIsAuthenticated_ReturnTrue()
        {
            #region Arrage
            UserAuthenticated("12345", "testuser", "mockAuthenticationType");
            #endregion

            #region Act
            var result = _sut.IsUserAuthenticated();
            #endregion

            #region Assert
            Assert.That(result, Is.True);
            #endregion
        }

        [Test]
        public void IsUserAuthenticated_WhenUserIsNotAuthenticated_ReturnFalse()
        {
            #region Arrage
            _userLogIn.AddIdentity(new ClaimsIdentity());
            #endregion

            #region Act
            var result = _sut.IsUserAuthenticated();
            #endregion

            #region Assert
            Assert.That(result, Is.False);
            #endregion
        }

        [Test]
        public async Task LogOut_WhenCallsSignOutAsyncMethod_ItLogsOut()
        {
            #region Arrage
            _signInManager.Setup(m => m.SignOutAsync())
                .Returns(Task.CompletedTask);
            #endregion

            #region Act
            await _sut.LogOut();
            #endregion

            #region Assert
            _signInManager.Verify(v => v.SignOutAsync(), Times.Once);
            #endregion
        }

        private List<User> GetFakeUserList(int total)
        {
            var users = new List<User>();

            for (int i = 0; i < total; i++)
            {
                var user = new User()
                {
                    Id = Guid.NewGuid().ToString(),
                    UserName = $"User {i}",
                    PasswordHash = Guid.NewGuid().ToString()
                };

                users.Add(user);
            }
            return users;
        }

        private class TestSignInResult
        {
            public static SignInResult SignStatus(string status)
            {
                return status == "Lockedout" ? SignInResult.LockedOut :
                       status == "NotAllowed" ? SignInResult.NotAllowed :
                       status == "RequiresTwoFactor" ? SignInResult.TwoFactorRequired :
                       status == "Succeeded" ? SignInResult.Success : SignInResult.Failed;
            }
        }

        private void UserAuthenticated(string nameIdentifier, string name, string authenticationType)
        {
            _userLogIn.AddIdentity(new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.NameIdentifier, nameIdentifier),
                new Claim(ClaimTypes.Name, name),
            }, authenticationType));
        }
    }
}
