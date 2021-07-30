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

            if (request == null)
            {
                response.IsSuccess = false;
                response.Message = "Некорректный, пустой запрос";
                return response;
            }

            if (request.DepartureWarehouseId == null && request.ArrivalWarehouseId == null)
            {
                response.IsSuccess = false;
                response.Message = "Не выбраны склады перемещения или расхода/прихода";
                return response;
            }
            else if (request.DepartureWarehouseId == request.ArrivalWarehouseId)
            {
                response.IsSuccess = false;
                response.Message = "Склад отправления не должен быть равен складу прибытия";
                return response;
            }

            if (request.InventoryItems.Count == 0)
            {
                response.IsSuccess = false;
                response.Message = "Не выбраны перемещаемые ТМЦ";
                return response;
            }

            var emptyInventoryItems = request.InventoryItems.Where(x => x.Count == 0);
            if (emptyInventoryItems.Count() > 0)
            {
                response.IsSuccess = false;

                var textIds = string.Join(", ", emptyInventoryItems.Select(x => x.Id));
                response.Message = $"Номенклатуры с id={textIds} выбранно кл-во равное 0";
                return response;
            }

            if (request.InventoryItems.GroupBy(x => x.Id).Select(x => x.Count()).Any(x => x > 1))
            {
                response.IsSuccess = false;
                response.Message = "В одном перемещении не могут быть две одинаковые номенклатуры";
                return response;
            }

            var warehouses = await _warehouseRepository.GetByIdsAsync(request.WarehouseIds);

            if (request.IsMovingDepartureWarehouse && warehouses.Any(x => x.Id == request.DepartureWarehouseId.Value) == false)
            {
                response.IsSuccess = false;
                response.Message = $"Склада отправления с id={request.DepartureWarehouseId.Value} не найдено";
                return response;
            }

            if (request.IsMovingArrivalWarehouse && warehouses.Any(x => x.Id == request.ArrivalWarehouseId.Value) == false)
            {
                response.IsSuccess = false;
                response.Message = $"Склада прибытия с id={request.ArrivalWarehouseId.Value} не найдено";
                return response;
            }

            var createInventoryItemIds = request.InventoryItems.Select(x => x.Id);
            var inventoryItems = await _inventoryItemRepository.GetByIdsAsync(createInventoryItemIds);
            var warehouseInventoryItems = await _warehouseInventoryItemRepository.GetAsync(request.WarehouseIds, createInventoryItemIds);

            foreach (var createInventoryItem in request.InventoryItems)
            {
                if (inventoryItems.Any(x => x.Id == createInventoryItem.Id) == false)
                {
                    response.IsSuccess = false;
                    response.Message = $"Номенклатуры с id={createInventoryItem.Id} не найдено";
                    return response;
                }

                if (request.IsMovingDepartureWarehouse)
                {
                    var departureWarehouseInventoryItem = warehouseInventoryItems.FirstOrDefault(x => x.WarehouseId == request.DepartureWarehouseId.Value && x.InventoryItemId == createInventoryItem.Id);
                    if (departureWarehouseInventoryItem == null)
                    {
                        response.IsSuccess = false;
                        response.Message = $"У выбраного склада отправления с id={request.DepartureWarehouseId.Value} нет ТМЦ с id={createInventoryItem.Id}";
                        return response;
                    }

                    var newCountDeparture = departureWarehouseInventoryItem.Count - createInventoryItem.Count;
                    if (newCountDeparture < 0)
                    {
                        response.IsSuccess = false;
                        response.Message = $"Нельзя расходовать нуменклатуру id={createInventoryItem.Id} в кол-ве: {createInventoryItem.Count}. Недостаточно остатков на складе, остаток: {departureWarehouseInventoryItem.Count}";
                        return response;
                    }
                }
            }

            return response;
        }
    }
}
