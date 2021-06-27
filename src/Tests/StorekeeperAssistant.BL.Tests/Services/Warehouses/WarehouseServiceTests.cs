using AutoFixture;
using Moq;
using NUnit.Framework;
using StorekeeperAssistant.Api.Models.Warehouses;
using StorekeeperAssistant.BL.Services.Warehouses;
using StorekeeperAssistant.BL.Services.Warehouses.Implementation;
using StorekeeperAssistant.DAL.Entities;
using StorekeeperAssistant.DAL.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StorekeeperAssistant.BL.Tests.Services.Warehouses
{
    public class WarehouseServiceTests
    {
        private Mock<IWarehouseRepository> _warehouseRepositoryMock;

        [SetUp]
        public void Setup()
        {
            _warehouseRepositoryMock = new Mock<IWarehouseRepository>();
        }

        [Test]
        public async Task Get_Many_Warehouses_Success_Test_Async()
        {
            // Arrange
            var warehouses = new Fixture().Build<Warehouse>().With(e => e.IsDeleted, false).CreateMany();
            _warehouseRepositoryMock.Setup(a => a.GetAsync()).ReturnsAsync(warehouses);

            IWarehouseService warehouseService = new WarehouseService(_warehouseRepositoryMock.Object);

            // Act
            var response = await warehouseService.GetAsync(new GetWarehouseRequest());

            // Assert
            Assert.AreEqual(warehouses.Count(), response.Warehouses.Count());
        }

        [Test]
        public async Task Get_Zero_Warehouses_Success_Test_Async()
        {
            // Arrange
            var warehouses = new List<Warehouse>();
            _warehouseRepositoryMock.Setup(a => a.GetAsync()).ReturnsAsync(warehouses);

            IWarehouseService warehouseService = new WarehouseService(_warehouseRepositoryMock.Object);

            // Act
            var response = await warehouseService.GetAsync(new GetWarehouseRequest());

            // Assert
            Assert.AreEqual(warehouses.Count(), response.Warehouses.Count());
        }
    }
}
