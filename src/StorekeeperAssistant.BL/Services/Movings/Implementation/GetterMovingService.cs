using StorekeeperAssistant.Api.Models.InventoryItems;
using StorekeeperAssistant.Api.Models.MovingDetails;
using StorekeeperAssistant.Api.Models.Movings;
using StorekeeperAssistant.Api.Models.Warehouses;
using StorekeeperAssistant.DAL.Entities;
using StorekeeperAssistant.DAL.Repositories;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StorekeeperAssistant.BL.Services.Movings.Implementation
{
    internal class GetterMovingService : IGetterMovingService
    {
        private readonly IMovingRepository _movingRepository;
        private readonly IInventoryItemRepository _inventoryItemRepository;

        public GetterMovingService(IMovingRepository movingRepository, IInventoryItemRepository inventoryItemRepository)
        {
            _movingRepository = movingRepository;
            _inventoryItemRepository = inventoryItemRepository;
        }

        private IEnumerable<InventoryItem> _inventoryItems;

        public async Task<GetMovingResponse> GetAsync(GetMovingRequest request)
        {
            var response = new GetMovingResponse { IsSuccess = true, Message = string.Empty };
            _inventoryItems =  await _inventoryItemRepository.GetAsync();
            var responseDTO = await _movingRepository.GetFullAsync(request.SkipCount, request.TakeCount);

            response.TotalCount = responseDTO.TotalCount;
            response.Movings = responseDTO.Movings.Select(MapToDTO);
            return response;
        }

        #region private
        private MovingDTO MapToDTO(Moving moving)
        {
            var movingModel = new MovingDTO
            {
                Id = moving.Id,
                DateTime = moving.DateTime,
                ArrivalWarehouse = MapToDTO(moving.ArrivalWarehouse),
                DepartureWarehouse = MapToDTO(moving.DepartureWarehouse),
                MovingDetails = moving.MovingDetails.Select(MapToDTO)
            };

            return movingModel;
        }

        private MovingDetailDTO MapToDTO(MovingDetail movingDetail)
        {
            var inventoryItem = _inventoryItems.First(x => x.Id == movingDetail.InventoryItemId);
            return new MovingDetailDTO
            {
                Id = movingDetail.Id,
                Count = movingDetail.Count,
                InventoryItem = new InventoryItemDTO
                {
                    Id = inventoryItem.Id,
                    Name = inventoryItem.Name
                }
            };
        }

        private WarehouseDTO MapToDTO(Warehouse warehouse)
        {
            if (warehouse == null) return null;
            return new WarehouseDTO
            {
                Id = warehouse.Id,
                Name = warehouse.Name
            };
        }
        #endregion
    }
}
