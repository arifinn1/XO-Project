using System.Threading.Tasks;
using XOProject.Controller;
using Microsoft.AspNetCore.Mvc;
using NUnit.Framework;
using Moq;

namespace XOProject.Tests
{
    class TradeControllerTests
    {
        private readonly Mock<ITradeRepository> _tradeRepositoryMock = new Mock<ITradeRepository>();

        private readonly TradeController _tradeController;

        public TradeControllerTests()
        {
            _tradeController = new TradeController(_tradeRepositoryMock.Object);
        }

        [Test]
        public async Task Post_InsertTrade()
        {
            var trad = new Trade
            {
                Symbol = "REL",
                NoOfShares = 60,
                Price = 8000,
                PortfolioId = 1,
                Action = "BUY"
            };

            // Arrange

            // Act
            var result = await _tradeController.Post(trad);

            // Assert
            Assert.NotNull(result);

            var createdResult = result as CreatedResult;
            Assert.NotNull(createdResult);
            Assert.AreEqual(201, createdResult.StatusCode);
        }

        [Test]
        public async Task Get_Trading()
        {
            var result = await _tradeController.GetAllTradings(1);

            // Assert
            Assert.NotNull(result);
        }

        [Test]
        public async Task Get_Analysis()
        {
            var result = await _tradeController.GetAnalysis(1);

            // Assert
            Assert.NotNull(result);
        }
    }
}
