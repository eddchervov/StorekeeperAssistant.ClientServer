using AutoFixture;
using Moq;
using NUnit.Framework;
using StorekeeperAssistant.Api.Models.InventoryItems;
using StorekeeperAssistant.Api.Models.WarehouseInventoryItems;
using StorekeeperAssistant.BL.Services.InventoryItems;
using StorekeeperAssistant.BL.Services.WarehouseInventoryItems;
using StorekeeperAssistant.BL.Services.WarehouseInventoryItems.Implementation;
using StorekeeperAssistant.DAL.Entities;
using StorekeeperAssistant.DAL.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StorekeeperAssistant.BL.Tests.Services.WarehouseInventoryItems
{
    public class WarehouseInventoryItemServiceTests
    {
        private Mock<IWarehouseInventoryItemRepository> _warehouseInventoryItemRepositoryMock;
        private Mock<IInventoryItemService> _inventoryItemServiceMock;

        [SetUp]
        public void Setup()
        {
            _warehouseInventoryItemRepositoryMock = new Mock<IWarehouseInventoryItemRepository>();
            _inventoryItemServiceMock = new Mock<IInventoryItemService>();
        }

        [Test]
        public async Task Get_WarehouseInventoryItems_Success_Test_Async()
        {
            // Arrange
            var warehouseId = new Fixture().Create<int>();
            var request = new GetWarehouseInventoryItemRequest { WarehouseId = warehouseId, DateTime = null };

            var inventoryItemDTOs = new Fixture().Build<InventoryItemDTO>().CreateMany();
            var inventoryItemResponse = new GetInventoryItemsResponse { InventoryItems = inventoryItemDTOs };

            var warehouseInventoryItems = new List<WarehouseInventoryItem>();
            foreach (var inventoryItemDTO in inventoryItemDTOs)
                warehouseInventoryItems.Add(new Fixture().Build<WarehouseInventoryItem>().With(e => e.WarehouseId, warehouseId).With(x=>x.InventoryItemId, inventoryItemDTO.Id).Create());

            _inventoryItemServiceMock
                .Setup(a => a.GetAsync(It.IsAny<GetInventoryItemsRequest>()))
                .ReturnsAsync(inventoryItemResponse);

            _warehouseInventoryItemRepositoryMock
              .Setup(a => a.GetLastesAsync(It.IsAny<IEnumerable<int>>(), It.IsAny<IEnumerable<int>>(), null))
              .ReturnsAsync(warehouseInventoryItems);

            IWarehouseInventoryItemService service = new WarehouseInventoryItemService(_warehouseInventoryItemRepositoryMock.Object, _inventoryItemServiceMock.Object);

            // Act
            var response = await service.GetAsync(request);

            // Assert
            Assert.IsTrue(response.IsSuccess);
            Assert.IsEmpty(response.Message);

            Assert.AreEqual(warehouseInventoryItems.Count, response.WarehouseInventoryItems.Count());
        }
    }
}
