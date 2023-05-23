using ChatRoomApp.API.External;
using ChatRoomApp.Data;
using ChatRoomApp.Helpers;
using ChatRoomApp.Helpers.Interfaces;
using ChatRoomApp.Models.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Moq;
using NUnit.Framework;
using System.Collections.Generic;
using System.Net.Http;

namespace ChatRoomApp.UnitTests
{
    [TestFixture]
    public class ChatBotStockTests
    {
        private readonly ChatBotStock _sut;
        private readonly Mock<HttpClient> _httpClient = new Mock<HttpClient>();

        public ChatBotStockTests()
        {
            _sut = new ChatBotStock(_httpClient.Object);
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
            Assert.That($"{stockStr} quote is $171.56 per share.", Is.EqualTo(result));
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
            Assert.That($"This stock does not have quote.", Is.EqualTo(result));
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