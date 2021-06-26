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

        public async Task<CreateMovingResponse> CreateAsync(CreateMovingRequest request)
        {
            var response = new CreateMovingResponse { IsSuccess = true, Message = string.Empty };
            var utcNow = DateTime.UtcNow;
            _warehouseInventoryItems = await _warehouseInventoryItemRepository.GetLastAsync(request.WarehouseIds, request.InventoryItems.Select(x => x.Id));

            try
            {
                using var transaction = _appDBContext.BeginTransaction();

                var moving = CreateMoving(request.DepartureWarehouseId, request.ArrivalWarehouseId, utcNow, request.IsDeleted);
                await _movingRepository.InsertAsync(moving);

                foreach (var inventoryItem in request.InventoryItems)
                {
                    var movingDetail = CreateMovingDetail(moving.Id, inventoryItem.Id, inventoryItem.Count);
                    await _movingDetailRepository.InsertAsync(movingDetail);

                    if (request.IsMovingDepartureWarehouse)
                    {
                        var warehouseInventoryItem = CreateDepartureWarehouseInventoryItem(request, inventoryItem, utcNow, moving.Id);
                        await _warehouseInventoryItemRepository.InsertAsync(warehouseInventoryItem);
                    }
                       
                    if (request.IsMovingArrivalWarehouse)
                    {
                        var warehouseInventoryItem = CreateArrivalWarehouseInventoryItem(request, inventoryItem, utcNow, moving.Id);
                        await _warehouseInventoryItemRepository.InsertAsync(warehouseInventoryItem);
                    }
                }

                await transaction.CommitAsync();
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = ex.ToString();
                return response;
            }

            return response;
        }

        #region private
        private WarehouseInventoryItem CreateDepartureWarehouseInventoryItem(CreateMovingRequest request, CreateInventoryItemDTO inventoryItem, DateTime utcNow, int movingId)
        {
            var departureWarehouseInventoryItem = GetWarehouseInventoryItem(request.DepartureWarehouseId.Value, inventoryItem.Id);
            var newCountDeparture = departureWarehouseInventoryItem.Count - inventoryItem.Count;

            return CreateWarehouseInventoryItem(inventoryItem.Id, utcNow, newCountDeparture, request.DepartureWarehouseId.Value, movingId);
        }

        private WarehouseInventoryItem CreateArrivalWarehouseInventoryItem(CreateMovingRequest request, CreateInventoryItemDTO inventoryItem, DateTime utcNow, int movingId)
        {
            var newCountArrival = inventoryItem.Count;
            var arrivalWarehouseInventoryItem = GetWarehouseInventoryItem(request.ArrivalWarehouseId.Value, inventoryItem.Id);
            if (arrivalWarehouseInventoryItem != null) newCountArrival += arrivalWarehouseInventoryItem.Count;

           return CreateWarehouseInventoryItem(inventoryItem.Id, utcNow, newCountArrival, request.ArrivalWarehouseId.Value, movingId);
        }

        private WarehouseInventoryItem GetWarehouseInventoryItem(int warehouseId, int inventoryItemId) 
        {
            return _warehouseInventoryItems.FirstOrDefault(x => x.WarehouseId == warehouseId && x.InventoryItemId == inventoryItemId);
        }

        private Moving CreateMoving(int? departureWarehouseId, int? arrivalWarehouseId, DateTime utcNow, bool isDeleted)
        {
            return new Moving
            {
                DepartureWarehouseId = departureWarehouseId,
                ArrivalWarehouseId = arrivalWarehouseId,
                IsDeleted = isDeleted,
                DateTime = utcNow
            };
        }

        private MovingDetail CreateMovingDetail(int movingId, int nomenclatureId, int count)
        {
            return new MovingDetail
            {
                MovingId = movingId,
                InventoryItemId = nomenclatureId,
                Count = count
            };
        }

        private WarehouseInventoryItem CreateWarehouseInventoryItem(int inventoryItemId, DateTime utcNow, int count, int departureWarehouseId, int movingId)
        {
            return new WarehouseInventoryItem
            {
                InventoryItemId = inventoryItemId,
                DateTime = utcNow,
                Count = count,
                WarehouseId = departureWarehouseId,
                MovingId = movingId,
                IsDeleted = false
            };
        }
        #endregion
    }
}
