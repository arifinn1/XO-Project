using System;
using System.Threading.Tasks;
using XOProject.Controller;
using Microsoft.AspNetCore.Mvc;
using NUnit.Framework;
using Moq;
using System.Collections.Generic;

namespace XOProject.Tests
{
    class PortofolioControllerTests
    {
        private readonly Mock<IPortfolioRepository> _portofolioRepositoryMock = new Mock<IPortfolioRepository>();

        private readonly PortfolioController _portfolioController;

        public PortofolioControllerTests()
        {
            _portfolioController = new PortfolioController(_portofolioRepositoryMock.Object);
        }

        [Test]
        public async Task Post_InsertPortofolio()
        {
            var porto = new Portfolio
            {
                Name = "Arifin"
            };

            // Act
            var result = await _portfolioController.Post(porto);

            // Assert
            Assert.NotNull(result);

            var createdResult = result as CreatedResult;
            Assert.NotNull(createdResult);
            Assert.AreEqual(201, createdResult.StatusCode);
        }

        [Test]
        public async Task Post_InsertPortofolioWithTrade()
        {
            var porto = new Portfolio
            {
                Name = "Arifin"
            };

            List<Trade> trad = new List<Trade>();
            trad.Add(new Trade{
                Symbol = "REL",
                NoOfShares = 60,
                Price = 8000,
                PortfolioId = porto.Id,
                Action = "BUY"
            });
            trad.Add(new Trade
            {
                Symbol = "REL",
                NoOfShares = 60,
                Price = 8000,
                PortfolioId = porto.Id,
                Action = "SELL"
            });

            porto.Trade = trad;

            // Act
            var result = await _portfolioController.Post(porto);

            // Assert
            Assert.NotNull(result);

            var createdResult = result as CreatedResult;
            Assert.NotNull(createdResult);
            Assert.AreEqual(201, createdResult.StatusCode);
        }

        [Test]
        public async Task Get_PortfolioInfo()
        {
            var result = await _portfolioController.GetPortfolioInfo(1);

            // Assert
            Assert.NotNull(result);
        }
    }
}
