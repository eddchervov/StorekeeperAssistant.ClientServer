using AutoFixture;
using Moq;
using NUnit.Framework;
using StorekeeperAssistant.Api.Models.Movings;
using StorekeeperAssistant.BL.Services.Movings;
using StorekeeperAssistant.BL.Services.Movings.Implementation;
using StorekeeperAssistant.DAL.Entities;
using StorekeeperAssistant.DAL.Models;
using StorekeeperAssistant.DAL.Repositories;
using System.Linq;
using System.Threading.Tasks;

namespace StorekeeperAssistant.BL.Tests.Services.Movings
{
    public class GetterMovingServiceTests
    {
        private Mock<IMovingRepository> _movingRepository;
        private Mock<IMovingDetailRepository> _movingDetailRepository;

        [SetUp]
        public void Setup()
        {
            _movingRepository = new Mock<IMovingRepository>();
            _movingDetailRepository = new Mock<IMovingDetailRepository>();
        }

        [Test]
        public async Task Get_Movings_Success_Test_Async()
        {
            // Arrange
            var request = new GetMovingRequest { SkipCount = 0, TakeCount = 1 };

            var movings = new Fixture().Build<Moving>().CreateMany(request.TakeCount).ToList();
            var totalCount = new Fixture().Create<int>();
            var movingDetailCount = 10;
            var movingResponse = new GetMovingsResponse { TotalCount = totalCount, Movings = movings };

            _movingRepository
                .Setup(a => a.GetFullAsync(request.SkipCount, request.TakeCount))
                .ReturnsAsync(movingResponse);

            IGetterMovingService service = new GetterMovingService(_movingRepository.Object);

            // Act
            var response = await service.GetAsync(request);

            // Assert
            Assert.IsTrue(response.IsSuccess);
            Assert.IsEmpty(response.Message);

            Assert.AreEqual(response.TotalCount, movingResponse.TotalCount);
            Assert.AreEqual(response.Movings.Count(), movingResponse.Movings.Count);

            foreach (var moving in response.Movings)
            {
                Assert.AreEqual(moving.MovingDetails.Count(), movingDetailCount);
            }
        }
    }
}
