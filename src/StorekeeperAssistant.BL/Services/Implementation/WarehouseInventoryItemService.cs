using StorekeeperAssistant.Api.Models.InventoryItem;
using StorekeeperAssistant.Api.Models.Nomenclature;
using StorekeeperAssistant.DAL.Repositories;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace StorekeeperAssistant.BL.Services.Implementation
{
    internal class WarehouseInventoryItemService : IWarehouseInventoryItemService
    {
        private readonly IWarehouseInventoryItemRepository _warehouseInventoryItemRepository;
        private readonly INomenclatureService _nomenclatureService;

        public WarehouseInventoryItemService(IWarehouseInventoryItemRepository warehouseInventoryItemRepository,
            INomenclatureService nomenclatureService)
        {
            _warehouseInventoryItemRepository = warehouseInventoryItemRepository;
            _nomenclatureService = nomenclatureService;
        }

        public async Task<GetWarehouseInventoryItemByWarehouseIdResponse> GetWarehouseInventoryItemByWarehouseIdAsync(GetWarehouseInventoryItemByWarehouseIdRequest request)
        {
            var response = new GetWarehouseInventoryItemByWarehouseIdResponse();

            var nomenclaturesResponse = await _nomenclatureService.GetNomenclaturesAsync(new GetNomenclaturesRequest());

            foreach (var nomenclature in nomenclaturesResponse.NomenclatureModels)
            {
                var warehouseInventoryItem = await _warehouseInventoryItemRepository.GetLastByWarehouseIdAndNomenclatureIdAsync(request.WarehouseId, nomenclature.Id, request.DateTime);
                if (warehouseInventoryItem != null && warehouseInventoryItem.Count != 0)
                {
                    var inventoryItemModel = new InventoryItemModel
                    {
                        Id = warehouseInventoryItem.Id,
                        Count = warehouseInventoryItem.Count,
                        NomenclatureModel = nomenclature
                    };

                    response.InventoryItemModels.Add(inventoryItemModel);
                }
            }

            return response;
        }
    }
}
