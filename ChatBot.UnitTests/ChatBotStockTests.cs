using Microsoft.Extensions.Configuration;
using Moq;

namespace ChatBot.UnitTests
{
    [TestFixture]
    public class ChatBotStockTests
    {
        private ChatBotStock _sut;
        private Mock<HttpClient> _httpClient = new Mock<HttpClient>();
        private Mock<IConfiguration> _configuration = new Mock<IConfiguration>();

        [SetUp]
        public void Setup()
        {
            _configuration
                .Setup(cong => cong.GetSection("ChatBotSettings:Url").Value)
                .Returns("https://stooq.com/q/l/");

            _sut = new ChatBotStock(_httpClient.Object, _configuration.Object);
        }

        [TearDown]
        public void TearDown()
        {
            _httpClient.Reset();
            _configuration.Reset();
        }

        [Test]
        public async Task GetBotStock_WhenPassAnExistingStockCode_ReturnStockCodeInformation()
        {
            #region Arrage
            var stockStr = "aapl.us";
            #endregion

            #region Act
            var result = await _sut.GetBotStock(stockStr);
            #endregion

            #region Assert
            Assert.That(result, Does.Contain(stockStr));
            #endregion
        }

        [Test]
        public async Task GetBotStock_WhenPassAnWrongStockCode_ReturnDoesNotExist()
        {
            #region Arrage
            var stockStr = "aa";
            #endregion

            #region Act
            var result = await _sut.GetBotStock(stockStr);
            #endregion

            #region Assert
            Assert.That(result, Is.EqualTo($"This stock does not have quote."));
            #endregion
        }

        [Test]
        public void IsStockCodeInvoked_WhenSendStockCodeKeyWord_ReturnTrue()
        {
            #region Arrage
            string stockCode = "/stock=";
            #endregion

            #region Act
            var result = _sut.IsStockCodeInvoked(stockCode);
            #endregion

            #region Assert
            Assert.IsTrue(result);
            #endregion
        }

        [Test]
        public void IsStockCodeInvoked_WhenNotSendStockCodeKeyWord_ReturnFalse()
        {
            #region Arrage
            string stockCode = "yes/stock=";
            #endregion

            #region Act
            var result = _sut.IsStockCodeInvoked(stockCode);
            #endregion

            #region Assert
            Assert.IsFalse(result);
            #endregion
        }
    }
}