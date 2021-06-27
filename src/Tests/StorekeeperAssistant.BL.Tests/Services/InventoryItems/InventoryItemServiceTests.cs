using AutoFixture;
using Moq;
using NUnit.Framework;
using StorekeeperAssistant.Api.Models.InventoryItems;
using StorekeeperAssistant.BL.Services.InventoryItems;
using StorekeeperAssistant.BL.Services.InventoryItems.Implementation;
using StorekeeperAssistant.DAL.Entities;
using StorekeeperAssistant.DAL.Repositories;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StorekeeperAssistant.BL.Tests.Services.InventoryItems
{
    public class InventoryItemServiceTests
    {
        private Mock<IInventoryItemRepository> _inventoryItemRepositoryMock;

        [SetUp]
        public void Setup()
        {
            _inventoryItemRepositoryMock = new Mock<IInventoryItemRepository>();
        }

        [Test]
        public async Task Get_Many_InventoryItems_Success_Test_Async()
        {
            // Arrange
            var inventoryItems = new Fixture().Build<InventoryItem>().With(e => e.IsDeleted, false).CreateMany();
            _inventoryItemRepositoryMock.Setup(a => a.GetAsync()).ReturnsAsync(inventoryItems);

            IInventoryItemService inventoryItemService = new InventoryItemService(_inventoryItemRepositoryMock.Object);

            // Act
            var response = await inventoryItemService.GetAsync(new GetInventoryItemsRequest());

            // Assert
            Assert.AreEqual(inventoryItems.Count(), response.InventoryItems.Count());
        }

        [Test]
        public async Task Get_Zero_InventoryItems_Success_Test_Async()
        {
            // Arrange
            var inventoryItems = new List<InventoryItem>();
            _inventoryItemRepositoryMock.Setup(a => a.GetAsync()).ReturnsAsync(inventoryItems);

            IInventoryItemService inventoryItemService = new InventoryItemService(_inventoryItemRepositoryMock.Object);

            // Act
            var response = await inventoryItemService.GetAsync(new GetInventoryItemsRequest());

            // Assert
            Assert.AreEqual(inventoryItems.Count(), response.InventoryItems.Count());
        }
    }
}