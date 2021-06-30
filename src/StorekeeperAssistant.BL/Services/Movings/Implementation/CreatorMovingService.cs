using StorekeeperAssistant.Api.Models.InventoryItems;
using StorekeeperAssistant.Api.Models.Movings;
using StorekeeperAssistant.DAL.DBContext;
using StorekeeperAssistant.DAL.Entities;
using StorekeeperAssistant.DAL.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StorekeeperAssistant.BL.Services.Movings.Implementation
{
    internal class CreatorMovingService : ICreatorMovingService
    {
        private readonly IMovingRepository _movingRepository;
        private readonly IWarehouseInventoryItemRepository _warehouseInventoryItemRepository;
        private readonly IMovingDetailRepository _movingDetailRepository;
        private readonly IAppDBContext _appDBContext;
        private IEnumerable<WarehouseInventoryItem> _warehouseInventoryItems;
        public CreatorMovingService(IMovingRepository movingRepository,
            IWarehouseInventoryItemRepository warehouseInventoryItemRepository,
            IMovingDetailRepository movingDetailRepository,
            IAppDBContext appDBContext)
        {
            _movingRepository = movingRepository;
            _warehouseInventoryItemRepository = warehouseInventoryItemRepository;
            _movingDetailRepository = movingDetailRepository;
            _appDBContext = appDBContext;
        }

        public async Task CreateAsync(CreateMovingRequest request)
        {
            var utcNow = DateTime.UtcNow;
            _warehouseInventoryItems = await _warehouseInventoryItemRepository.GetLastesAsync(request.WarehouseIds, request.InventoryItems.Select(x => x.Id));

            using var transaction = _appDBContext.BeginTransaction();

            var moving = await CreateMovingAsync(request, utcNow);

            foreach (var inventoryItem in request.InventoryItems)
            {
                await CreateMovingDetailAsync(moving.Id, inventoryItem);
                await CreateDepartureWarehouseInventoryItemAsync(request, inventoryItem, moving.Id, utcNow);
                await CreateArrivalWarehouseInventoryItemAsync(request, inventoryItem, moving.Id, utcNow);
            }

            await transaction.CommitAsync();
        }

        #region private
        private async Task<Moving> CreateMovingAsync(CreateMovingRequest request, DateTime utcNow)
        {
            var moving = CreateMoving(request, utcNow);
            await _movingRepository.InsertAsync(moving);

            return moving;
        }

        private async Task CreateMovingDetailAsync(int movingId, CreateInventoryItemDTO inventoryItem)
        {
            var movingDetail = CreateMovingDetail(movingId, inventoryItem);
            await _movingDetailRepository.InsertAsync(movingDetail);
        }

        private async Task CreateDepartureWarehouseInventoryItemAsync(CreateMovingRequest request, CreateInventoryItemDTO inventoryItem, int movingId, DateTime utcNow)
        {
            if (request.IsMovingDepartureWarehouse == false) return;

            var warehouseInventoryItem = CreateDepartureWarehouseInventoryItem(inventoryItem, request.DepartureWarehouseId.Value, movingId, utcNow);
            await _warehouseInventoryItemRepository.InsertAsync(warehouseInventoryItem);
        }

        private async Task CreateArrivalWarehouseInventoryItemAsync(CreateMovingRequest request, CreateInventoryItemDTO inventoryItem, int movingId, DateTime utcNow)
        {
            if (request.IsMovingArrivalWarehouse == false) return;

            var warehouseInventoryItem = CreateArrivalWarehouseInventoryItem(inventoryItem, request.ArrivalWarehouseId.Value, movingId, utcNow);
            await _warehouseInventoryItemRepository.InsertAsync(warehouseInventoryItem);
        }

        private WarehouseInventoryItem CreateDepartureWarehouseInventoryItem(CreateInventoryItemDTO inventoryItem, int departureWarehouseId, int movingId, DateTime utcNow)
        {
            var departureWarehouseInventoryItem = GetWarehouseInventoryItem(departureWarehouseId, inventoryItem.Id);
            var newCountDeparture = departureWarehouseInventoryItem.Count - inventoryItem.Count;

            return CreateWarehouseInventoryItem(inventoryItem.Id, utcNow, newCountDeparture, departureWarehouseId, movingId);
        }

        private WarehouseInventoryItem CreateArrivalWarehouseInventoryItem(CreateInventoryItemDTO inventoryItem, int arrivalWarehouseId, int movingId, DateTime utcNow)
        {
            var newCountArrival = inventoryItem.Count;
            var arrivalWarehouseInventoryItem = GetWarehouseInventoryItem(arrivalWarehouseId, inventoryItem.Id);
            if (arrivalWarehouseInventoryItem != null) newCountArrival += arrivalWarehouseInventoryItem.Count;

            return CreateWarehouseInventoryItem(inventoryItem.Id, utcNow, newCountArrival, arrivalWarehouseId, movingId);
        }

        private WarehouseInventoryItem GetWarehouseInventoryItem(int warehouseId, int inventoryItemId)
        {
            return _warehouseInventoryItems.FirstOrDefault(x => x.WarehouseId == warehouseId && x.InventoryItemId == inventoryItemId);
        }

        private Moving CreateMoving(CreateMovingRequest request, DateTime utcNow)
        {
            return new Moving
            {
                DepartureWarehouseId = request.DepartureWarehouseId,
                ArrivalWarehouseId = request.ArrivalWarehouseId,
                IsDeleted = request.IsDeleted,
                DateTime = utcNow
            };
        }

        private MovingDetail CreateMovingDetail(int movingId, CreateInventoryItemDTO inventoryItem)
        {
            return new MovingDetail
            {
                MovingId = movingId,
                InventoryItemId = inventoryItem.Id,
                Count = inventoryItem.Count
            };
        }

        private WarehouseInventoryItem CreateWarehouseInventoryItem(int inventoryItemId, DateTime utcNow, int count, int warehouseId, int movingId)
        {
            return new WarehouseInventoryItem
            {
                InventoryItemId = inventoryItemId,
                DateTime = utcNow,
                Count = count,
                WarehouseId = warehouseId,
                MovingId = movingId
            };
        }
        #endregion
    }
}
