using AutoFixture;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using NUnit.Framework;
using StorekeeperAssistant.Api.Models.Movings;
using StorekeeperAssistant.BL.Services.Movings;
using StorekeeperAssistant.BL.Services.Movings.Implementation;
using StorekeeperAssistant.DAL.DBContext;
using StorekeeperAssistant.DAL.DBContext.Implementation;
using StorekeeperAssistant.DAL.Entities;
using StorekeeperAssistant.DAL.Repositories;
using StorekeeperAssistant.DAL.Repositories.Implementation;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace StorekeeperAssistant.BL.Tests.Services.Movings
{
    public class CreatorMovingServiceTests
    {
        private IAppDBContext _appDBContext;
        private IMovingRepository _movingRepository;
        private IMovingDetailRepository _movingDetailRepository;
        private IWarehouseInventoryItemRepository _warehouseInventoryItemRepository;

        [SetUp]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<AppDBContext>()
               .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
               // don't raise the error warning us that the in memory db doesn't support transactions
               .ConfigureWarnings(x => x.Ignore(InMemoryEventId.TransactionIgnoredWarning))
               .Options;

            _appDBContext = new AppDBContext(options);
            _movingRepository = new MovingRepository(_appDBContext);
            _movingDetailRepository = new MovingDetailRepository(_appDBContext);
            _warehouseInventoryItemRepository = new WarehouseInventoryItemRepository(_appDBContext);
        }

        [Test]
        public async Task Create_Moving_Where_Adding_To_Warehouse_Outside_And_Exist_Last_Remain_Success_Test_Async()
        {
            // Arrange
            var request = new Fixture().Build<CreateMovingRequest>()
                .Without(x => x.DepartureWarehouseId)
                .Create();

            foreach (var inventoryItem in request.InventoryItems)
            {
                var arrivalWarehouseInventoryItem = new WarehouseInventoryItem
                {
                    Count = new Fixture().Create<int>(),
                    DateTime = DateTime.Now,
                    InventoryItemId = inventoryItem.Id,
                    WarehouseId = request.ArrivalWarehouseId.Value
                };
                await _warehouseInventoryItemRepository.InsertAsync(arrivalWarehouseInventoryItem);
            }

            ICreatorMovingService service = new CreatorMovingService(_movingRepository, _warehouseInventoryItemRepository, _movingDetailRepository, _appDBContext);

            // Act
            var response = await service.CreateAsync(request);

            // Assert
            Assert.IsTrue(response.IsSuccess);
            Assert.IsEmpty(response.Message);

            var moving = await _movingRepository.FirstOrDefaultAsync(x => x.DepartureWarehouseId == request.DepartureWarehouseId && x.ArrivalWarehouseId == request.ArrivalWarehouseId);
            Assert.IsNotNull(moving);

            var movingDetails = _movingDetailRepository.Where(x => x.MovingId == moving.Id);
            Assert.AreEqual(movingDetails.Count(), request.InventoryItems.Count);

            foreach (var movingDetail in movingDetails)
            {
                var inventoryItemDTO = request.InventoryItems.FirstOrDefault(x => x.Id == movingDetail.InventoryItemId);
                Assert.AreEqual(movingDetail.Count, inventoryItemDTO.Count);
            }

            var warehouseInventoryItems = _warehouseInventoryItemRepository.Where(x => x.MovingId == moving.Id);
            Assert.AreEqual(warehouseInventoryItems.Count(), request.InventoryItems.Count);

            foreach (var warehouseInventoryItem in warehouseInventoryItems)
            {
                Assert.AreEqual(warehouseInventoryItem.WarehouseId, request.ArrivalWarehouseId.Value);

                var previousRemain = await _warehouseInventoryItemRepository.FirstOrDefaultAsync(x =>
                    x.InventoryItemId == warehouseInventoryItem.InventoryItemId
                    && x.WarehouseId == request.ArrivalWarehouseId
                    && x.MovingId != moving.Id);
                var createInventoryItemDTO = request.InventoryItems.FirstOrDefault(x => x.Id == warehouseInventoryItem.InventoryItemId);

                Assert.NotNull(previousRemain);
                Assert.AreEqual(previousRemain.Count, warehouseInventoryItem.Count - createInventoryItemDTO.Count);
            }
        }

        [Test]
        public async Task Create_Moving_Where_Adding_To_Warehouse_Outside_And_Not_Exist_Last_Remain_Success_Test_Async()
        {
            // Arrange
            var request = new Fixture().Build<CreateMovingRequest>()
                .Without(x => x.DepartureWarehouseId)
                .Create();

            ICreatorMovingService service = new CreatorMovingService(_movingRepository, _warehouseInventoryItemRepository, _movingDetailRepository, _appDBContext);

            // Act
            var response = await service.CreateAsync(request);

            // Assert
            Assert.IsTrue(response.IsSuccess);
            Assert.IsEmpty(response.Message);

            var moving = await _movingRepository.FirstOrDefaultAsync(x => x.DepartureWarehouseId == request.DepartureWarehouseId && x.ArrivalWarehouseId == request.ArrivalWarehouseId);
            Assert.IsNotNull(moving);

            var movingDetails = _movingDetailRepository.Where(x => x.MovingId == moving.Id);
            Assert.AreEqual(movingDetails.Count(), request.InventoryItems.Count);

            foreach (var movingDetail in movingDetails)
            {
                var inventoryItemDTO = request.InventoryItems.FirstOrDefault(x => x.Id == movingDetail.InventoryItemId);
                Assert.AreEqual(movingDetail.Count, inventoryItemDTO.Count);
            }

            var warehouseInventoryItems = _warehouseInventoryItemRepository.Where(x => x.MovingId == moving.Id);
            Assert.AreEqual(warehouseInventoryItems.Count(), request.InventoryItems.Count);

            foreach (var warehouseInventoryItem in warehouseInventoryItems)
            {
                Assert.AreEqual(warehouseInventoryItem.WarehouseId, request.ArrivalWarehouseId.Value);

                var previousRemain = await _warehouseInventoryItemRepository.FirstOrDefaultAsync(x =>
                    x.InventoryItemId == warehouseInventoryItem.InventoryItemId
                    && x.WarehouseId == request.ArrivalWarehouseId
                    && x.MovingId != moving.Id);
                var createInventoryItemDTO = request.InventoryItems.FirstOrDefault(x => x.Id == warehouseInventoryItem.InventoryItemId);

                Assert.IsNull(previousRemain);
                Assert.AreEqual(0, warehouseInventoryItem.Count - createInventoryItemDTO.Count);
            }
        }

        [Test]
        public async Task Create_Moving_Where_Remove_From_Warehouse_And_Exist_Last_Remain_Success_Test_Async()
        {
            // Arrange
            var request = new Fixture().Build<CreateMovingRequest>()
                .Without(x => x.ArrivalWarehouseId)
                .Create();

            foreach (var inventoryItem in request.InventoryItems)
            {
                var departureWarehouseInventoryItem = new WarehouseInventoryItem
                {
                    Count = new Fixture().Create<int>(),
                    DateTime = DateTime.Now,
                    InventoryItemId = inventoryItem.Id,
                    WarehouseId = request.DepartureWarehouseId.Value
                };
                await _warehouseInventoryItemRepository.InsertAsync(departureWarehouseInventoryItem);
            }

            ICreatorMovingService service = new CreatorMovingService(_movingRepository, _warehouseInventoryItemRepository, _movingDetailRepository, _appDBContext);

            // Act
            var response = await service.CreateAsync(request);

            // Assert
            Assert.IsTrue(response.IsSuccess);
            Assert.IsEmpty(response.Message);

            var moving = await _movingRepository.FirstOrDefaultAsync(x => x.DepartureWarehouseId == request.DepartureWarehouseId && x.ArrivalWarehouseId == request.ArrivalWarehouseId);
            Assert.IsNotNull(moving);

            var movingDetails = _movingDetailRepository.Where(x => x.MovingId == moving.Id);
            Assert.AreEqual(movingDetails.Count(), request.InventoryItems.Count);

            foreach (var movingDetail in movingDetails)
            {
                var inventoryItemDTO = request.InventoryItems.FirstOrDefault(x => x.Id == movingDetail.InventoryItemId);
                Assert.AreEqual(movingDetail.Count, inventoryItemDTO.Count);
            }

            var warehouseInventoryItems = _warehouseInventoryItemRepository.Where(x => x.MovingId == moving.Id);
            Assert.AreEqual(warehouseInventoryItems.Count(), request.InventoryItems.Count);

            foreach (var warehouseInventoryItem in warehouseInventoryItems)
            {
                Assert.AreEqual(warehouseInventoryItem.WarehouseId, request.DepartureWarehouseId.Value);

                var previousRemain = await _warehouseInventoryItemRepository.FirstOrDefaultAsync(x =>
                    x.InventoryItemId == warehouseInventoryItem.InventoryItemId
                    && x.WarehouseId == request.DepartureWarehouseId
                    && x.MovingId != moving.Id);
                var createInventoryItemDTO = request.InventoryItems.FirstOrDefault(x => x.Id == warehouseInventoryItem.InventoryItemId);

                Assert.NotNull(previousRemain);
                Assert.AreEqual(previousRemain.Count, warehouseInventoryItem.Count + createInventoryItemDTO.Count);
            }
        }

        [Test]
        public async Task Create_Moving_Between_Warehouses_Where_Arrival_Warehouse_Exist_Last_Remain_Success_Test_Async()
        {
            // Arrange
            var request = new Fixture().Build<CreateMovingRequest>()
                .Create();

            for (int i = 0; i < request.InventoryItems.Count; i++)
            {
                var departureWarehouseInventoryItem = new WarehouseInventoryItem
                {
                    Count = request.InventoryItems[i].Count + new Fixture().Create<int>(),
                    DateTime = DateTime.Now,
                    InventoryItemId = request.InventoryItems[i].Id,
                    WarehouseId = request.DepartureWarehouseId.Value
                };
                await _warehouseInventoryItemRepository.InsertAsync(departureWarehouseInventoryItem);
            }

            for (int i = 0; i < request.InventoryItems.Count; i++)
            {
                var arrivalWarehouseInventoryItem = new WarehouseInventoryItem
                {
                    Count = new Fixture().Create<int>(),
                    DateTime = DateTime.Now,
                    InventoryItemId = request.InventoryItems[i].Id,
                    WarehouseId = request.ArrivalWarehouseId.Value
                };
                await _warehouseInventoryItemRepository.InsertAsync(arrivalWarehouseInventoryItem);
            }

            ICreatorMovingService service = new CreatorMovingService(_movingRepository, _warehouseInventoryItemRepository, _movingDetailRepository, _appDBContext);

            // Act
            var response = await service.CreateAsync(request);

            // Assert
            Assert.IsTrue(response.IsSuccess);
            Assert.IsEmpty(response.Message);

            var moving = await _movingRepository.FirstOrDefaultAsync(x => x.DepartureWarehouseId == request.DepartureWarehouseId && x.ArrivalWarehouseId == request.ArrivalWarehouseId);
            Assert.IsNotNull(moving);

            var movingDetails = _movingDetailRepository.Where(x => x.MovingId == moving.Id);
            Assert.AreEqual(movingDetails.Count(), request.InventoryItems.Count);

            foreach (var movingDetail in movingDetails)
            {
                var inventoryItemDTO = request.InventoryItems.FirstOrDefault(x => x.Id == movingDetail.InventoryItemId);
                Assert.AreEqual(movingDetail.Count, inventoryItemDTO.Count);
            }

            var departureWarehouseInventoryItems = _warehouseInventoryItemRepository.Where(x => x.MovingId == moving.Id && x.WarehouseId == request.DepartureWarehouseId);
            var arrivalWarehouseInventoryItems = _warehouseInventoryItemRepository.Where(x => x.MovingId == moving.Id && x.WarehouseId == request.ArrivalWarehouseId);

            Assert.AreEqual(departureWarehouseInventoryItems.Count(), request.InventoryItems.Count);
            Assert.AreEqual(arrivalWarehouseInventoryItems.Count(), request.InventoryItems.Count);
           
            foreach (var departureWarehouseInventoryItem in departureWarehouseInventoryItems)
            {
                Assert.AreEqual(departureWarehouseInventoryItem.WarehouseId, request.DepartureWarehouseId.Value);

                var previousRemain = await _warehouseInventoryItemRepository.FirstOrDefaultAsync(x =>
                    x.InventoryItemId == departureWarehouseInventoryItem.InventoryItemId
                    && x.WarehouseId == request.DepartureWarehouseId
                    && x.MovingId != moving.Id);
                var createInventoryItemDTO = request.InventoryItems.FirstOrDefault(x => x.Id == departureWarehouseInventoryItem.InventoryItemId);

                Assert.NotNull(previousRemain);
                Assert.AreEqual(departureWarehouseInventoryItem.Count, previousRemain.Count - createInventoryItemDTO.Count);
            }

            foreach (var arrivalWarehouseInventoryItem in arrivalWarehouseInventoryItems)
            {
                Assert.AreEqual(arrivalWarehouseInventoryItem.WarehouseId, request.ArrivalWarehouseId.Value);

                var previousRemain = await _warehouseInventoryItemRepository.FirstOrDefaultAsync(x =>
                    x.InventoryItemId == arrivalWarehouseInventoryItem.InventoryItemId
                    && x.WarehouseId == request.ArrivalWarehouseId
                    && x.MovingId != moving.Id);
                var createInventoryItemDTO = request.InventoryItems.FirstOrDefault(x => x.Id == arrivalWarehouseInventoryItem.InventoryItemId);

                Assert.NotNull(previousRemain);
                Assert.AreEqual(arrivalWarehouseInventoryItem.Count, previousRemain.Count + createInventoryItemDTO.Count);
            }
        }

        [Test]
        public async Task Create_Moving_Between_Warehouses_Where_Arrival_Warehouse_Not_Exist_Last_Remain_Success_Test_Async()
        {
            // Arrange
            var request = new Fixture().Build<CreateMovingRequest>()
                .Create();

            for (int i = 0; i < request.InventoryItems.Count; i++)
            {
                var departureWarehouseInventoryItem = new WarehouseInventoryItem
                {
                    Count = request.InventoryItems[i].Count + new Fixture().Create<int>(),
                    DateTime = DateTime.Now,
                    InventoryItemId = request.InventoryItems[i].Id,
                    WarehouseId = request.DepartureWarehouseId.Value
                };
                await _warehouseInventoryItemRepository.InsertAsync(departureWarehouseInventoryItem);
            }

            ICreatorMovingService service = new CreatorMovingService(_movingRepository, _warehouseInventoryItemRepository, _movingDetailRepository, _appDBContext);

            // Act
            var response = await service.CreateAsync(request);

            // Assert
            Assert.IsTrue(response.IsSuccess);
            Assert.IsEmpty(response.Message);

            var moving = await _movingRepository.FirstOrDefaultAsync(x => x.DepartureWarehouseId == request.DepartureWarehouseId && x.ArrivalWarehouseId == request.ArrivalWarehouseId);
            Assert.IsNotNull(moving);

            var movingDetails = _movingDetailRepository.Where(x => x.MovingId == moving.Id);
            Assert.AreEqual(movingDetails.Count(), request.InventoryItems.Count);

            foreach (var movingDetail in movingDetails)
            {
                var inventoryItemDTO = request.InventoryItems.FirstOrDefault(x => x.Id == movingDetail.InventoryItemId);
                Assert.AreEqual(movingDetail.Count, inventoryItemDTO.Count);
            }

            var departureWarehouseInventoryItems = _warehouseInventoryItemRepository.Where(x => x.MovingId == moving.Id && x.WarehouseId == request.DepartureWarehouseId);
            var arrivalWarehouseInventoryItems = _warehouseInventoryItemRepository.Where(x => x.MovingId == moving.Id && x.WarehouseId == request.ArrivalWarehouseId);

            Assert.AreEqual(departureWarehouseInventoryItems.Count(), request.InventoryItems.Count);
            Assert.AreEqual(arrivalWarehouseInventoryItems.Count(), request.InventoryItems.Count);

            foreach (var departureWarehouseInventoryItem in departureWarehouseInventoryItems)
            {
                Assert.AreEqual(departureWarehouseInventoryItem.WarehouseId, request.DepartureWarehouseId.Value);

                var previousRemain = await _warehouseInventoryItemRepository.FirstOrDefaultAsync(x =>
                    x.InventoryItemId == departureWarehouseInventoryItem.InventoryItemId
                    && x.WarehouseId == request.DepartureWarehouseId
                    && x.MovingId != moving.Id);
                var createInventoryItemDTO = request.InventoryItems.FirstOrDefault(x => x.Id == departureWarehouseInventoryItem.InventoryItemId);

                Assert.NotNull(previousRemain);
                Assert.AreEqual(departureWarehouseInventoryItem.Count, previousRemain.Count - createInventoryItemDTO.Count);
            }

            foreach (var arrivalWarehouseInventoryItem in arrivalWarehouseInventoryItems)
            {
                Assert.AreEqual(arrivalWarehouseInventoryItem.WarehouseId, request.ArrivalWarehouseId.Value);

                var previousRemain = await _warehouseInventoryItemRepository.FirstOrDefaultAsync(x =>
                    x.InventoryItemId == arrivalWarehouseInventoryItem.InventoryItemId
                    && x.WarehouseId == request.ArrivalWarehouseId
                    && x.MovingId != moving.Id);
                var createInventoryItemDTO = request.InventoryItems.FirstOrDefault(x => x.Id == arrivalWarehouseInventoryItem.InventoryItemId);

                Assert.IsNull(previousRemain);
                Assert.AreEqual(arrivalWarehouseInventoryItem.Count, createInventoryItemDTO.Count);
            }
        }
    }
}
