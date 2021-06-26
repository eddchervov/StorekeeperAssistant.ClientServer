using StorekeeperAssistant.Api.Models.InventoryItems;
using StorekeeperAssistant.Api.Models.WarehouseInventoryItems;
using StorekeeperAssistant.BL.Services.InventoryItems;
using StorekeeperAssistant.DAL.Entities;
using StorekeeperAssistant.DAL.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StorekeeperAssistant.BL.Services.WarehouseInventoryItems.Implementation
{
    internal class WarehouseInventoryItemService : IWarehouseInventoryItemService
    {
        private readonly IWarehouseInventoryItemRepository _warehouseInventoryItemRepository;
        private readonly IInventoryItemService _inventoryItemService;
        private IEnumerable<WarehouseInventoryItem> _warehouseInventoryItems;

        public WarehouseInventoryItemService(IWarehouseInventoryItemRepository warehouseInventoryItemRepository,
            IInventoryItemService inventoryItemService)
        {
            _warehouseInventoryItemRepository = warehouseInventoryItemRepository;
            _inventoryItemService = inventoryItemService;
        }

        public async Task<GetWarehouseInventoryItemResponse> GetAsync(GetWarehouseInventoryItemRequest request)
        {
            var response = new GetWarehouseInventoryItemResponse();

            var inventoryItemResponse = await _inventoryItemService.GetAsync(new GetInventoryItemsRequest());

            _warehouseInventoryItems = await GetWarehouseInventoryItemsAsync(request.WarehouseId, inventoryItemResponse.InventoryItems.Select(x => x.Id), request.DateTime);

            var warehouseInventoryItemDTOs = new List<WarehouseInventoryItemDTO>();
            foreach (var inventoryItem in inventoryItemResponse.InventoryItems)
            {
                var warehouseInventoryItem = GetWarehouseInventoryItem(request.WarehouseId, inventoryItem.Id);
                if (warehouseInventoryItem != null)
                {
                    var warehouseInventoryItemDTO = MapToDTO(warehouseInventoryItem, inventoryItem);
                    warehouseInventoryItemDTOs.Add(warehouseInventoryItemDTO);
                }
            }

            response.WarehouseInventoryItems = warehouseInventoryItemDTOs;
            return response;
        }

        #region private
        private WarehouseInventoryItemDTO MapToDTO(WarehouseInventoryItem warehouseInventoryItem, InventoryItemDTO inventoryItem)
        {
            return new WarehouseInventoryItemDTO
            {
                Id = warehouseInventoryItem.Id,
                Count = warehouseInventoryItem.Count,
                InventoryItem = inventoryItem
            };
        }

        private WarehouseInventoryItem GetWarehouseInventoryItem(int warehouseId, int inventoryItemId)
        {
            return _warehouseInventoryItems.FirstOrDefault(x => x.Id == warehouseId && x.InventoryItemId == inventoryItemId);
        }

        private async Task<IEnumerable<WarehouseInventoryItem>> GetWarehouseInventoryItemsAsync(int warehouseId, IEnumerable<int> inventoryItemIds, DateTime? dateTime)
        {
            return await _warehouseInventoryItemRepository.GetLastAsync(new List<int>() { warehouseId }, inventoryItemIds, dateTime);
        }
        #endregion
    }
}
