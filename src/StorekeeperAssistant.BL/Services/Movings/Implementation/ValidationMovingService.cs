using StorekeeperAssistant.Api.Models;
using StorekeeperAssistant.Api.Models.Movings;
using StorekeeperAssistant.DAL.Entities;
using StorekeeperAssistant.DAL.Repositories;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace StorekeeperAssistant.BL.Services.Movings.Implementation
{
    internal class ValidationMovingService : IValidationMovingService
    {
        private readonly IWarehouseRepository _warehouseRepository;
        private readonly IWarehouseInventoryItemRepository _warehouseInventoryItemRepository;
        private readonly IInventoryItemRepository _inventoryItemRepository;

        public ValidationMovingService(IWarehouseRepository warehouseRepository,
            IWarehouseInventoryItemRepository warehouseInventoryItemRepository,
            IInventoryItemRepository nomenclatureRepository)
        {
            _warehouseRepository = warehouseRepository;
            _warehouseInventoryItemRepository = warehouseInventoryItemRepository;
            _inventoryItemRepository = nomenclatureRepository;
        }

        public async Task<BaseResponse> ValidationAsync(CreateMovingRequest request)
        {
            var response = new BaseResponse { IsSuccess = true, Message = string.Empty };

            if (request.DepartureWarehouseId == null && request.ArrivalWarehouseId == null)
            {
                response.IsSuccess = false;
                response.Message = $"Не выбраны склады перемещения или расхода/прихода";
                return response;
            }
            else if (request.DepartureWarehouseId == request.ArrivalWarehouseId)
            {
                response.IsSuccess = false;
                response.Message = $"Склад отправления не должен быть равен складу прибытия";
                return response;
            }

            Warehouse departureWarehouse = null;
            if (request.DepartureWarehouseId != null)
            {
                departureWarehouse = await _warehouseRepository.GetByIdAsync(request.DepartureWarehouseId.Value);
                if (departureWarehouse == null)
                {
                    response.IsSuccess = false;
                    response.Message = $"Склада отправления с id={request.DepartureWarehouseId.Value} не найдено";
                    return response;
                }
            }

            if (request.ArrivalWarehouseId != null)
            {
                Warehouse arrivalWarehouse = await _warehouseRepository.GetByIdAsync(request.ArrivalWarehouseId.Value);
                if (arrivalWarehouse == null)
                {
                    response.IsSuccess = false;
                    response.Message = $"Склада прибытия с id={request.ArrivalWarehouseId.Value} не найдено";
                    return response;
                }
            }

            if (request.InventoryItems.Count == 0)
            {
                response.IsSuccess = false;
                response.Message = $"Не выбраны перемещаемые ТМЦ";
                return response;
            }

            var inventoryItemIds = new List<int>();

            foreach (var inventoryItemModel in request.InventoryItems)
            {
                var inventoryItem = await _inventoryItemRepository.GetByIdAsync(inventoryItemModel.Id);
                if (inventoryItem == null)
                {
                    response.IsSuccess = false;
                    response.Message = $"Нуменклатуры с id={inventoryItemModel.Id} не найдено";
                    return response;
                }

                if (inventoryItemModel.Count == 0)
                {
                    response.IsSuccess = false;
                    response.Message = $"Нуменклатуры с id={inventoryItemModel.Id} выбранно кл-во равное 0";
                    return response;
                }

                if (inventoryItemIds.Contains(inventoryItem.Id))
                {
                    response.IsSuccess = false;
                    response.Message = $"В одном перемещении не могут быть две одинаковые нуменклатуры";
                    return response;
                }

                if (request.DepartureWarehouseId != null)
                {
                    var departureWarehouseInventoryItem = await _warehouseInventoryItemRepository.GetLastAsync(request.DepartureWarehouseId.Value, inventoryItem.Id);
                    var newCountDeparture = departureWarehouseInventoryItem.Count - inventoryItemModel.Count;
                    if (newCountDeparture < 0)
                    {
                        response.IsSuccess = false;
                        response.Message = $"Нельзя расходовать нуменклатуру id={inventoryItem.Id} в кол-ве: {inventoryItemModel.Count}, недостаточно остатков на складе, остаток: {departureWarehouseInventoryItem.Count}";
                        return response;
                    }
                }

                inventoryItemIds.Add(inventoryItem.Id);

                if (departureWarehouse != null)
                {
                    var warehouseInventoryItem = await _warehouseInventoryItemRepository.GetLastAsync(departureWarehouse.Id, inventoryItemModel.Id);
                    if (warehouseInventoryItem == null)
                    {
                        response.IsSuccess = false;
                        response.Message = $"У выбраного склада отправления с id={departureWarehouse.Id} нет ТМЦ с id={inventoryItemModel.Id}";
                        return response;
                    }

                    if (warehouseInventoryItem.Count < inventoryItemModel.Count)
                    {
                        response.IsSuccess = false;
                        response.Message = $"У выбраного ТМЦ с id={inventoryItemModel.Id} запрашивается больше товаров, чем имеется на складе отправления с id={departureWarehouse.Id}";
                        return response;
                    }
                }
            }

            return response;
        }
    }
}
