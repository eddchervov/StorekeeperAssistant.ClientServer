using StorekeeperAssistant.Api.Models;
using StorekeeperAssistant.Api.Models.Movings;
using StorekeeperAssistant.DAL.Entities;
using StorekeeperAssistant.DAL.Repositories;
using System.Collections.Generic;
using System.Linq;
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
            IInventoryItemRepository inventoryItemRepository)
        {
            _warehouseRepository = warehouseRepository;
            _warehouseInventoryItemRepository = warehouseInventoryItemRepository;
            _inventoryItemRepository = inventoryItemRepository;
        }

        public BaseResponse Validation(GetMovingRequest request)
        {
            var response = new DeleteMovingResponse { IsSuccess = true, Message = string.Empty };

            if (request.TakeCount < 1)
            {
                response.IsSuccess = false;
                response.Message = $"{nameof(request.TakeCount)} не может быть меньше 1";
                return response;
            }

            if (request.SkipCount < 0)
            {
                response.IsSuccess = false;
                response.Message = $"{nameof(request.SkipCount)} не может быть меньше 0";
                return response;
            }

            return response;
        }

        public DeleteMovingResponse ErrorResponse(int movingId)
        {
            var response = new DeleteMovingResponse { IsSuccess = true, Message = string.Empty };

            response.IsSuccess = false;
            response.Message = $"Перемещение с id={movingId} не найдено";
            return response;
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

            if (request.InventoryItems.Count == 0)
            {
                response.IsSuccess = false;
                response.Message = $"Не выбраны перемещаемые ТМЦ";
                return response;
            }

            var warehouses = await _warehouseRepository.GetByIdsAsync(request.WarehouseIds);

            Warehouse departureWarehouse = null;
            if (request.IsMovingDepartureWarehouse)
            {
                departureWarehouse = warehouses.FirstOrDefault(x => x.Id == request.DepartureWarehouseId.Value);
                if (departureWarehouse == null)
                {
                    response.IsSuccess = false;
                    response.Message = $"Склада отправления с id={request.DepartureWarehouseId.Value} не найдено";
                    return response;
                }
            }

            if (request.IsMovingArrivalWarehouse)
            {
                var arrivalWarehouse = warehouses.FirstOrDefault(x => x.Id == request.ArrivalWarehouseId.Value);
                if (arrivalWarehouse == null)
                {
                    response.IsSuccess = false;
                    response.Message = $"Склада прибытия с id={request.ArrivalWarehouseId.Value} не найдено";
                    return response;
                }
            }

            var createInventoryItemIds = request.InventoryItems.Select(x => x.Id);
            var inventoryItems = await _inventoryItemRepository.GetByIdsAsync(createInventoryItemIds);
            var warehouseInventoryItems = await _warehouseInventoryItemRepository.GetAsync(request.WarehouseIds, createInventoryItemIds);

            var inventoryItemIds = new List<int>();
            foreach (var createInventoryItem in request.InventoryItems)
            {
                var inventoryItem = inventoryItems.FirstOrDefault(x => x.Id == createInventoryItem.Id);
                if (inventoryItem == null)
                {
                    response.IsSuccess = false;
                    response.Message = $"Нуменклатуры с id={createInventoryItem.Id} не найдено";
                    return response;
                }

                if (createInventoryItem.Count == 0)
                {
                    response.IsSuccess = false;
                    response.Message = $"Нуменклатуры с id={createInventoryItem.Id} выбранно кл-во равное 0";
                    return response;
                }

                if (inventoryItemIds.Contains(inventoryItem.Id))
                {
                    response.IsSuccess = false;
                    response.Message = $"В одном перемещении не могут быть две одинаковые нуменклатуры";
                    return response;
                }

                if (request.IsMovingDepartureWarehouse)
                {
                    var departureWarehouseInventoryItem = warehouseInventoryItems.FirstOrDefault(x => x.WarehouseId == request.DepartureWarehouseId.Value && x.InventoryItemId == inventoryItem.Id);
                    var newCountDeparture = departureWarehouseInventoryItem.Count - createInventoryItem.Count;
                    if (newCountDeparture < 0)
                    {
                        response.IsSuccess = false;
                        response.Message = $"Нельзя расходовать нуменклатуру id={inventoryItem.Id} в кол-ве: {createInventoryItem.Count}. Недостаточно остатков на складе, остаток: {departureWarehouseInventoryItem.Count}";
                        return response;
                    }
                }

                inventoryItemIds.Add(inventoryItem.Id);

                if (request.IsMovingDepartureWarehouse)
                {
                    var warehouseInventoryItem = warehouseInventoryItems.FirstOrDefault(x => x.WarehouseId == departureWarehouse.Id && x.InventoryItemId == createInventoryItem.Id);
                    if (warehouseInventoryItem == null)
                    {
                        response.IsSuccess = false;
                        response.Message = $"У выбраного склада отправления с id={departureWarehouse.Id} нет ТМЦ с id={createInventoryItem.Id}";
                        return response;
                    }

                    if (warehouseInventoryItem.Count < createInventoryItem.Count)
                    {
                        response.IsSuccess = false;
                        response.Message = $"У выбраного ТМЦ с id={createInventoryItem.Id} запрашивается больше товаров, чем имеется на складе отправления с id={departureWarehouse.Id}";
                        return response;
                    }
                }
            }

            return response;
        }
    }
}
