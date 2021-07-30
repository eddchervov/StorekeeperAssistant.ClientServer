using AutoFixture;
using Moq;
using NUnit.Framework;
using StorekeeperAssistant.Api.Models.Movings;
using StorekeeperAssistant.BL.Services.Movings;
using StorekeeperAssistant.BL.Services.Movings.Implementation;
using StorekeeperAssistant.DAL.Entities;
using StorekeeperAssistant.DAL.Models;
using StorekeeperAssistant.DAL.Repositories;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StorekeeperAssistant.BL.Tests.Services.Movings
{
    public class GetterMovingServiceTests
    {
        private Mock<IMovingRepository> _movingRepository;
        private Mock<IInventoryItemRepository> _inventoryItemRepository;

        [SetUp]
        public void Setup()
        {
            _movingRepository = new Mock<IMovingRepository>();
            _inventoryItemRepository = new Mock<IInventoryItemRepository>();
        }

        [Test]
        public async Task Get_Movings_Success_Test_Async()
        {
            // Arrange
            var request = new GetMovingRequest { SkipCount = 0, TakeCount = 1 };

            var movingDetailCount = 10;
            var movings = new List<Moving>();
            for (int i = 0; i < request.TakeCount; i++)
            {
                var moving = new Moving
                {
                    Id = 1,
                    MovingDetails = new List<MovingDetail>
                    {
                        new MovingDetail
                        {
                            Count = movingDetailCount / 2,
                            InventoryItemId = 1,
                            MovingId = 1
                        },
                        new MovingDetail
                        {
                            Count = movingDetailCount / 2,
                            InventoryItemId = 2,
                            MovingId = 1
                        }
                    }
                };

                movings.Add(moving);
            }

            var totalCount = new Fixture().Create<int>();

            var movingResponse = new GetMovingsResponse { TotalCount = totalCount, Movings = movings };

            _movingRepository
                .Setup(a => a.GetFullAsync(request.SkipCount, request.TakeCount))
                    .ReturnsAsync(movingResponse);

            var inventoryItem_1 = new Fixture().Build<InventoryItem>()
               .With(x => x.Id, 1)
               .Create();
            var inventoryItem_2 = new Fixture().Build<InventoryItem>()
                .With(x => x.Id, 2)
                .Create();
            IEnumerable<InventoryItem> inventoryItems = new List<InventoryItem>() { inventoryItem_1, inventoryItem_2 };
           
            _inventoryItemRepository
               .Setup(a => a.GetAsync())
                   .ReturnsAsync(inventoryItems);

            IGetterMovingService service = new GetterMovingService(_movingRepository.Object, _inventoryItemRepository.Object);

            // Act
            var response = await service.GetAsync(request);

            // Assert
            Assert.IsTrue(response.IsSuccess);
            Assert.IsEmpty(response.Message);

            Assert.AreEqual(response.TotalCount, movingResponse.TotalCount);
            Assert.AreEqual(response.Movings.Count(), movingResponse.Movings.Count);

            foreach (var moving in response.Movings)
            {
                Assert.AreEqual(moving.MovingDetails.Count(), 2);
            }
        }
    }
}
