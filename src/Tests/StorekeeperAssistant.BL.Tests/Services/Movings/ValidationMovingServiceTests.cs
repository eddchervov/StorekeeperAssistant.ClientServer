using AutoFixture;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using NUnit.Framework;
using StorekeeperAssistant.Api.Models.Movings;
using StorekeeperAssistant.BL.Services.Movings;
using StorekeeperAssistant.BL.Services.Movings.Implementation;
using StorekeeperAssistant.DAL.DBContext.Implementation;
using StorekeeperAssistant.DAL.Entities;
using StorekeeperAssistant.DAL.Repositories;
using StorekeeperAssistant.DAL.Repositories.Implementation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StorekeeperAssistant.BL.Tests.Services.Movings
{
    public class ValidationMovingServiceTests
    {
        private IWarehouseRepository _warehouseRepository;
        private IWarehouseInventoryItemRepository _warehouseInventoryItemRepository;
        private IInventoryItemRepository _inventoryItemRepository;
        private IMovingRepository _movingRepository;

        [SetUp]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<AppDBContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                // don't raise the error warning us that the in memory db doesn't support transactions
                .ConfigureWarnings(x => x.Ignore(InMemoryEventId.TransactionIgnoredWarning))
                .Options;

            var appDBContext = new AppDBContext(options);

            _warehouseRepository = new WarehouseRepository(appDBContext);
            _warehouseInventoryItemRepository = new WarehouseInventoryItemRepository(appDBContext);
            _inventoryItemRepository = new InventoryItemRepository(appDBContext);
            _movingRepository = new MovingRepository(appDBContext);
        }

        #region Success_Test
        [Test]
        public async Task CreateMoving_Operation_Moving_Success_Test_Async()
        {
            // Arrange
            var warehous_1 = new Fixture().Build<Warehouse>()
                .With(x => x.IsDeleted, false)
                .Create();
            var warehous_2 = new Fixture().Build<Warehouse>()
                .With(x => x.IsDeleted, false)
                .Create();

            await _warehouseRepository.InsertAsync(warehous_1);
            await _warehouseRepository.InsertAsync(warehous_2);

            var request = new Fixture().Build<CreateMovingRequest>()
                .With(x => x.DepartureWarehouseId, warehous_1.Id)
                .With(x => x.ArrivalWarehouseId, warehous_2.Id)
                .Create();

            foreach (var requestInventoryItem in request.InventoryItems) requestInventoryItem.Count = 100;

            var firstCreateInventoryItem = request.InventoryItems.First();

            for (int i = 0; i < request.InventoryItems.Count; i++)
            {
                var inventoryItem = new Fixture().Build<InventoryItem>()
                    .With(x => x.IsDeleted, false)
                    .With(x => x.Id, request.InventoryItems[i].Id)
                    .Create();

                await _inventoryItemRepository.InsertAsync(inventoryItem);
            }

            var inventoryItems = (await _inventoryItemRepository.GetAsync()).ToList();

            var moving = new Moving
            {
                IsDeleted = false,
                DepartureWarehouseId = warehous_1.Id,
                ArrivalWarehouseId = warehous_2.Id,
                DateTime = DateTime.Now
            };

            await _movingRepository.InsertAsync(moving);

            for (int i = 0; i < inventoryItems.Count; i++)
            {
                var warehouseInventoryItem = new WarehouseInventoryItem
                {
                    DateTime = DateTime.Now,
                    Count = 1000,
                    InventoryItemId = inventoryItems[i].Id,
                    MovingId = moving.Id,
                    WarehouseId = warehous_1.Id
                };

                await _warehouseInventoryItemRepository.InsertAsync(warehouseInventoryItem);
            }

            IValidationMovingService service = new ValidationMovingService(_warehouseRepository, _warehouseInventoryItemRepository, _inventoryItemRepository);

            // Act
            var response = await service.ValidationAsync(request);

            // Assert
            Assert.IsTrue(response.IsSuccess);
            Assert.IsEmpty(response.Message);
        }

        [Test]
        public async Task CreateMoving_Operation_Coming_Success_Test_Async()
        {
            // Arrange
            var arrivalWarehouse = new Fixture().Build<Warehouse>()
                .With(x => x.IsDeleted, false)
                .Create();

            await _warehouseRepository.InsertAsync(arrivalWarehouse);

            var request = new Fixture().Build<CreateMovingRequest>()
                .With(x => x.ArrivalWarehouseId, arrivalWarehouse.Id)
                .Without(x => x.DepartureWarehouseId)
                .Create();

            foreach (var requestInventoryItem in request.InventoryItems) requestInventoryItem.Count = 100;

            var firstCreateInventoryItem = request.InventoryItems.First();

            for (int i = 0; i < request.InventoryItems.Count; i++)
            {
                var inventoryItem = new Fixture().Build<InventoryItem>()
                    .With(x => x.IsDeleted, false)
                    .With(x => x.Id, request.InventoryItems[i].Id)
                    .Create();

                await _inventoryItemRepository.InsertAsync(inventoryItem);
            }

            var inventoryItems = (await _inventoryItemRepository.GetAsync()).ToList();

            var moving = new Moving
            {
                IsDeleted = false,
                DepartureWarehouseId = null,
                ArrivalWarehouseId = arrivalWarehouse.Id,
                DateTime = DateTime.Now
            };

            await _movingRepository.InsertAsync(moving);

            for (int i = 0; i < inventoryItems.Count; i++)
            {
                var warehouseInventoryItem = new WarehouseInventoryItem
                {
                    DateTime = DateTime.Now,
                    Count = 1000,
                    InventoryItemId = inventoryItems[i].Id,
                    MovingId = moving.Id,
                    WarehouseId = arrivalWarehouse.Id
                };

                await _warehouseInventoryItemRepository.InsertAsync(warehouseInventoryItem);
            }

            IValidationMovingService service = new ValidationMovingService(_warehouseRepository, _warehouseInventoryItemRepository, _inventoryItemRepository);

            // Act
            var response = await service.ValidationAsync(request);

            // Assert
            Assert.IsTrue(response.IsSuccess);
            Assert.IsEmpty(response.Message);
        }

        [Test]
        public async Task CreateMoving_Operation_Consumption_Success_Test_Async()
        {
            // Arrange
            var departureWarehouse = new Fixture().Build<Warehouse>()
                .With(x => x.IsDeleted, false)
                .Create();

            await _warehouseRepository.InsertAsync(departureWarehouse);

            var request = new Fixture().Build<CreateMovingRequest>()
                .With(x => x.DepartureWarehouseId, departureWarehouse.Id)
                .Without(x => x.ArrivalWarehouseId)
                .Create();

            foreach (var requestInventoryItem in request.InventoryItems) requestInventoryItem.Count = 100;

            var firstCreateInventoryItem = request.InventoryItems.First();

            for (int i = 0; i < request.InventoryItems.Count; i++)
            {
                var inventoryItem = new Fixture().Build<InventoryItem>()
                    .With(x => x.IsDeleted, false)
                    .With(x => x.Id, request.InventoryItems[i].Id)
                    .Create();

                await _inventoryItemRepository.InsertAsync(inventoryItem);
            }

            var inventoryItems = (await _inventoryItemRepository.GetAsync()).ToList();

            var moving = new Moving
            {
                IsDeleted = false,
                DepartureWarehouseId = departureWarehouse.Id,
                ArrivalWarehouseId = null,
                DateTime = DateTime.Now
            };

            await _movingRepository.InsertAsync(moving);

            for (int i = 0; i < inventoryItems.Count; i++)
            {
                var warehouseInventoryItem = new WarehouseInventoryItem
                {
                    DateTime = DateTime.Now,
                    Count = 1000,
                    InventoryItemId = inventoryItems[i].Id,
                    MovingId = moving.Id,
                    WarehouseId = departureWarehouse.Id
                };

                await _warehouseInventoryItemRepository.InsertAsync(warehouseInventoryItem);
            }

            IValidationMovingService service = new ValidationMovingService(_warehouseRepository, _warehouseInventoryItemRepository, _inventoryItemRepository);

            // Act
            var response = await service.ValidationAsync(request);

            // Assert
            Assert.IsTrue(response.IsSuccess);
            Assert.IsEmpty(response.Message);
        }
        #endregion

        #region Error_Test
        [Test]
        public async Task CreateMoving_Empty_Request_Error_Test_Async()
        {
            // Arrange
            CreateMovingRequest request = null;

            IValidationMovingService service = new ValidationMovingService(_warehouseRepository, _warehouseInventoryItemRepository, _inventoryItemRepository);

            // Act
            var response = await service.ValidationAsync(request);

            // Assert
            Assert.IsFalse(response.IsSuccess);
            Assert.AreEqual(response.Message, "Некорректный, пустой запрос");
        }

        [Test]
        public async Task CreateMoving_ArrivalWarehouseId_Equal_Null_And_DepartureWarehouseId_Equal_Null_Error_Test_Async()
        {
            // Arrange
            var request = new CreateMovingRequest();

            IValidationMovingService service = new ValidationMovingService(_warehouseRepository, _warehouseInventoryItemRepository, _inventoryItemRepository);

            // Act
            var response = await service.ValidationAsync(request);

            // Assert
            Assert.IsFalse(response.IsSuccess);
            Assert.AreEqual(response.Message, "Не выбраны склады перемещения или расхода/прихода");
        }

        [Test]
        public async Task CreateMoving_ArrivalWarehouseId_Equal_DepartureWarehouseId_Error_Test_Async()
        {
            // Arrange
            var request = new CreateMovingRequest { ArrivalWarehouseId = 1, DepartureWarehouseId = 1 };

            IValidationMovingService service = new ValidationMovingService(_warehouseRepository, _warehouseInventoryItemRepository, _inventoryItemRepository);

            // Act
            var response = await service.ValidationAsync(request);

            // Assert
            Assert.IsFalse(response.IsSuccess);
            Assert.AreEqual(response.Message, "Склад отправления не должен быть равен складу прибытия");
        }

        [Test]
        public async Task CreateMoving_Not_Found_InventoryItems_Error_Test_Async()
        {
            // Arrange
            var request = new CreateMovingRequest { ArrivalWarehouseId = 1, DepartureWarehouseId = 2 };

            IValidationMovingService service = new ValidationMovingService(_warehouseRepository, _warehouseInventoryItemRepository, _inventoryItemRepository);

            // Act
            var response = await service.ValidationAsync(request);

            // Assert
            Assert.IsFalse(response.IsSuccess);
            Assert.AreEqual(response.Message, "Не выбраны перемещаемые ТМЦ");
        }

        [Test]
        public async Task CreateMoving_InventoryItem_Count_0_Error_Test_Async()
        {
            // Arrange
            var warehous_1 = new Fixture().Build<Warehouse>()
                .With(x => x.IsDeleted, false)
                .Create();
            var warehous_2 = new Fixture().Build<Warehouse>()
                .With(x => x.IsDeleted, false)
                .Create();

            await _warehouseRepository.InsertAsync(warehous_1);
            await _warehouseRepository.InsertAsync(warehous_2);

            var request = new Fixture().Build<CreateMovingRequest>()
                .With(x => x.ArrivalWarehouseId, warehous_1.Id)
                .With(x => x.DepartureWarehouseId, warehous_2.Id)
                .Create();
            foreach (var inventoryItem in request.InventoryItems) inventoryItem.Count = 0;

            var inventoryItems = new Fixture().Build<InventoryItem>()
                .With(x => x.IsDeleted, false)
                .CreateMany()
                .ToList();

            for (int i = 0; i < inventoryItems.Count(); i++)
            {
                inventoryItems[i].Id = request.InventoryItems[i].Id;
                await _inventoryItemRepository.InsertAsync(inventoryItems[i]);
            }

            IValidationMovingService service = new ValidationMovingService(_warehouseRepository, _warehouseInventoryItemRepository, _inventoryItemRepository);

            // Act
            var response = await service.ValidationAsync(request);

            // Assert
            Assert.IsFalse(response.IsSuccess);
            var textIds = string.Join(", ", request.InventoryItems.Where(x => x.Count == 0).Select(x => x.Id));
            Assert.AreEqual(response.Message, $"Номенклатуры с id={textIds} выбранно кл-во равное 0");
        }

        [Test]
        public async Task CreateMoving_InventoryItem_Identical_Id_Error_Test_Async()
        {
            // Arrange
            var warehous_1 = new Fixture().Build<Warehouse>()
                .With(x => x.IsDeleted, false)
                .Create();
            var warehous_2 = new Fixture().Build<Warehouse>()
                .With(x => x.IsDeleted, false)
                .Create();

            await _warehouseRepository.InsertAsync(warehous_1);
            await _warehouseRepository.InsertAsync(warehous_2);

            var request = new Fixture().Build<CreateMovingRequest>()
                .With(x => x.ArrivalWarehouseId, warehous_1.Id)
                .With(x => x.DepartureWarehouseId, warehous_2.Id)
                .Create();
            foreach (var inventoryItem in request.InventoryItems) inventoryItem.Id = 1;

            IValidationMovingService service = new ValidationMovingService(_warehouseRepository, _warehouseInventoryItemRepository, _inventoryItemRepository);

            // Act
            var response = await service.ValidationAsync(request);

            // Assert
            Assert.IsFalse(response.IsSuccess);
            var textIds = string.Join(", ", request.InventoryItems.Where(x => x.Count == 0).Select(x => x.Id));
            Assert.AreEqual(response.Message, "В одном перемещении не могут быть две одинаковые номенклатуры");
        }

        [Test]
        public async Task CreateMoving_DepartureWarehouseId_Not_Found_Error_Test_Async()
        {
            // Arrange
            var request = new Fixture().Build<CreateMovingRequest>()
                .With(x => x.ArrivalWarehouseId, 1)
                .With(x => x.DepartureWarehouseId, 2)
                .With(x => x.IsDeleted, false)
                .Create();

            IValidationMovingService service = new ValidationMovingService(_warehouseRepository, _warehouseInventoryItemRepository, _inventoryItemRepository);

            // Act
            var response = await service.ValidationAsync(request);

            // Assert
            Assert.IsFalse(response.IsSuccess);
            Assert.AreEqual(response.Message, $"Склада отправления с id={request.DepartureWarehouseId.Value} не найдено");
        }

        [Test]
        public async Task CreateMoving_ArrivalWarehouseId_Not_Found_Error_Test_Async()
        {
            // Arrange
            var request = new Fixture().Build<CreateMovingRequest>()
                .With(x => x.ArrivalWarehouseId, 1)
                .With(x => x.IsDeleted, false)
                .Without(x => x.DepartureWarehouseId)
                .Create();

            IValidationMovingService service = new ValidationMovingService(_warehouseRepository, _warehouseInventoryItemRepository, _inventoryItemRepository);

            // Act
            var response = await service.ValidationAsync(request);

            // Assert
            Assert.IsFalse(response.IsSuccess);
            Assert.AreEqual(response.Message, $"Склада прибытия с id={request.ArrivalWarehouseId.Value} не найдено");
        }

        [Test]
        public async Task CreateMoving_InventoryItem_Not_Found_Error_Test_Async()
        {
            // Arrange
            var warehous_1 = new Fixture().Build<Warehouse>()
                .With(x => x.IsDeleted, false)
                .Create();
            var warehous_2 = new Fixture().Build<Warehouse>()
                .With(x => x.IsDeleted, false)
                .Create();

            await _warehouseRepository.InsertAsync(warehous_1);
            await _warehouseRepository.InsertAsync(warehous_2);

            var request = new Fixture().Build<CreateMovingRequest>()
                .With(x => x.ArrivalWarehouseId, warehous_1.Id)
                .With(x => x.DepartureWarehouseId, warehous_2.Id)
                .Create();
            var firstInventoryItem = request.InventoryItems.First();

            IValidationMovingService service = new ValidationMovingService(_warehouseRepository, _warehouseInventoryItemRepository, _inventoryItemRepository);

            // Act
            var response = await service.ValidationAsync(request);

            // Assert
            Assert.IsFalse(response.IsSuccess);
            Assert.AreEqual(response.Message, $"Номенклатуры с id={firstInventoryItem.Id} не найдено");
        }

        [Test]
        public async Task CreateMoving_DepartureWarehouse_Does_Not_Have_InventoryItem_Error_Test_Async()
        {
            // Arrange
            var warehous_1 = new Fixture().Build<Warehouse>()
                .With(x => x.IsDeleted, false)
                .Create();
            var warehous_2 = new Fixture().Build<Warehouse>()
                .With(x => x.IsDeleted, false)
                .Create();

            await _warehouseRepository.InsertAsync(warehous_1);
            await _warehouseRepository.InsertAsync(warehous_2);

            var request = new Fixture().Build<CreateMovingRequest>()
                .With(x => x.DepartureWarehouseId, warehous_1.Id)
                .With(x => x.ArrivalWarehouseId, warehous_2.Id)
                .Create();

            foreach (var requestInventoryItem in request.InventoryItems)
            {
                requestInventoryItem.Count = 1001;
            }

            var firstCreateInventoryItem = request.InventoryItems.First();

            for (int i = 0; i < request.InventoryItems.Count; i++)
            {
                var inventoryItem = new Fixture().Build<InventoryItem>()
                    .With(x => x.IsDeleted, false)
                    .With(x => x.Id, request.InventoryItems[i].Id)
                    .Create();

                await _inventoryItemRepository.InsertAsync(inventoryItem);
            }

            var firstInventoryItem = request.InventoryItems.First();

            IValidationMovingService service = new ValidationMovingService(_warehouseRepository, _warehouseInventoryItemRepository, _inventoryItemRepository);

            // Act
            var response = await service.ValidationAsync(request);

            // Assert
            Assert.IsFalse(response.IsSuccess);
            Assert.AreEqual(response.Message, $"У выбраного склада отправления с id={request.DepartureWarehouseId.Value} нет ТМЦ с id={firstInventoryItem.Id}");
        }

        [Test]
        public async Task CreateMoving_Insufficient_Stock_Balance_Error_Test_Async()
        {
            // Arrange
            var warehous_1 = new Fixture().Build<Warehouse>()
                .With(x => x.IsDeleted, false)
                .Create();
            var warehous_2 = new Fixture().Build<Warehouse>()
                .With(x => x.IsDeleted, false)
                .Create();

            await _warehouseRepository.InsertAsync(warehous_1);
            await _warehouseRepository.InsertAsync(warehous_2);

            var request = new Fixture().Build<CreateMovingRequest>()
                .With(x => x.DepartureWarehouseId, warehous_1.Id)
                .With(x => x.ArrivalWarehouseId, warehous_2.Id)
                .Create();

            foreach (var requestInventoryItem in request.InventoryItems) requestInventoryItem.Count = 1001;

            var firstCreateInventoryItem = request.InventoryItems.First();

            for (int i = 0; i < request.InventoryItems.Count; i++)
            {
                var inventoryItem = new Fixture().Build<InventoryItem>()
                    .With(x => x.IsDeleted, false)
                    .With(x => x.Id, request.InventoryItems[i].Id)
                    .Create();

                await _inventoryItemRepository.InsertAsync(inventoryItem);
            }

            var inventoryItems = (await _inventoryItemRepository.GetAsync()).ToList();

            var moving = new Moving
            {
                IsDeleted = false,
                DepartureWarehouseId = warehous_1.Id,
                ArrivalWarehouseId = warehous_2.Id,
                DateTime = DateTime.Now
            };

            await _movingRepository.InsertAsync(moving);

            for (int i = 0; i < inventoryItems.Count; i++)
            {
                var warehouseInventoryItem = new WarehouseInventoryItem
                {
                    DateTime = DateTime.Now,
                    Count = 1000,
                    InventoryItemId = inventoryItems[i].Id,
                    MovingId = moving.Id,
                    WarehouseId = warehous_1.Id
                };

                await _warehouseInventoryItemRepository.InsertAsync(warehouseInventoryItem);
            }


            IValidationMovingService service = new ValidationMovingService(_warehouseRepository, _warehouseInventoryItemRepository, _inventoryItemRepository);

            var warehouseInventoryItems = await _warehouseInventoryItemRepository.GetAsync(new List<int> { warehous_1.Id }, inventoryItems.Select(x => x.Id));
            var firstInventoryItem = inventoryItems.First();
            var firstWarehouseInventoryItem = warehouseInventoryItems.First(x => x.InventoryItemId == firstInventoryItem.Id && x.WarehouseId == warehous_1.Id && x.MovingId == moving.Id);

            // Act
            var response = await service.ValidationAsync(request);

            // Assert
            Assert.IsFalse(response.IsSuccess);
            Assert.AreEqual(response.Message, $"Нельзя расходовать нуменклатуру id={firstInventoryItem.Id} в кол-ве: {firstCreateInventoryItem.Count}. Недостаточно остатков на складе, остаток: {firstWarehouseInventoryItem.Count}");
        }
        #endregion
    }
}
