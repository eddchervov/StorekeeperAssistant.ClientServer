using AutoFixture;
using Moq;
using NUnit.Framework;
using StorekeeperAssistant.Api.Models.WarehouseInventoryItems;
using StorekeeperAssistant.BL.Services.WarehouseInventoryItems;
using StorekeeperAssistant.BL.Services.WarehouseInventoryItems.Implementation;
using StorekeeperAssistant.DAL.Entities;
using StorekeeperAssistant.DAL.Repositories;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace StorekeeperAssistant.BL.Tests.Services.WarehouseInventoryItems
{
    public class ValidationWarehouseInventoryItemServiceTests
    {
        private Mock<IWarehouseRepository> _warehouseRepositoryMock;

        [SetUp]
        public void Setup()
        {
            _warehouseRepositoryMock = new Mock<IWarehouseRepository>();
        }

        [Test]
        public async Task Validation_Is_Exist_Warehouse_Success_Test_Async()
        {
            // Arrange
            var warehouseId = new Fixture().Create<int>();
            var request = new GetWarehouseInventoryItemRequest { WarehouseId = warehouseId };
            var warehouse = new Fixture().Build<Warehouse>().With(e => e.IsDeleted, false).With(e => e.Id, warehouseId).Create();
            _warehouseRepositoryMock.Setup(a => a.GetByIdAsync(warehouseId)).ReturnsAsync(warehouse);

            IValidationWarehouseInventoryItemService service = new ValidationWarehouseInventoryItemService(_warehouseRepositoryMock.Object);

            // Act
            var response = await service.ValidationAsync(request);

            // Assert
            Assert.IsTrue(response.IsSuccess);
            Assert.IsEmpty(response.Message);
        }

        [Test]
        public async Task Validation_Is_Not_Exist_Warehouse_Success_Test_Async()
        {
            // Arrange
            var warehouseId = new Fixture().Create<int>();
            var request = new GetWarehouseInventoryItemRequest { WarehouseId = warehouseId };
            var warehouse = new Fixture().Build<Warehouse>().With(e => e.IsDeleted, false).Create();
            _warehouseRepositoryMock.Setup(a => a.GetByIdAsync(warehouseId)).ReturnsAsync((Warehouse) null);

            IValidationWarehouseInventoryItemService service = new ValidationWarehouseInventoryItemService(_warehouseRepositoryMock.Object);

            // Act
            var response = await service.ValidationAsync(request);

            // Assert
            Assert.IsFalse(response.IsSuccess);
            Assert.AreEqual(response.Message, $"Склада с id={request.WarehouseId} не найдено");
        }
    }
}
