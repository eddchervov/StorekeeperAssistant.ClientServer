using StorekeeperAssistant.Api.Models;
using StorekeeperAssistant.Api.Models.Moving;
using StorekeeperAssistant.DAL.Entities;
using StorekeeperAssistant.DAL.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StorekeeperAssistant.BL.Services.Implementation
{
    internal class ValidationMovingService : IValidationMovingService
    {
        private readonly IWarehouseRepository _warehouseRepository;
        private readonly IWarehouseInventoryItemRepository _warehouseInventoryItemRepository;
        private readonly INomenclatureRepository _nomenclatureRepository;

        public ValidationMovingService(IWarehouseRepository warehouseRepository,
            IWarehouseInventoryItemRepository warehouseInventoryItemRepository,
            INomenclatureRepository nomenclatureRepository)
        {
            _warehouseRepository = warehouseRepository;
            _warehouseInventoryItemRepository = warehouseInventoryItemRepository;
            _nomenclatureRepository = nomenclatureRepository;
        }

        public async Task<BaseResponse> ValidationCreateMovingAsync(CreateMovingRequest request)
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

            if (request.CreateInventoryItemModels.Count == 0)
            {
                response.IsSuccess = false;
                response.Message = $"Не выбраны перемещаемые ТМЦ";
                return response;
            }

            var nomenclatureIds = new List<int>();

            foreach (var inventoryItemModel in request.CreateInventoryItemModels)
            {
                var nomenclature = await _nomenclatureRepository.GetByIdAsync(inventoryItemModel.Id);
                if (nomenclature == null)
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

                if (nomenclatureIds.Contains(nomenclature.Id))
                {
                    response.IsSuccess = false;
                    response.Message = $"В одном перемещении не могут быть две одинаковые нуменклатуры";
                    return response;
                }

                if (request.DepartureWarehouseId != null)
                {
                    var departureWarehouseInventoryItem = await _warehouseInventoryItemRepository.GetLastAsync(request.DepartureWarehouseId.Value, nomenclature.Id);
                    var newCountDeparture = departureWarehouseInventoryItem.Count - inventoryItemModel.Count;
                    if (newCountDeparture < 0)
                    {
                        response.IsSuccess = false;
                        response.Message = $"Нельзя расходовать нуменклатуру id={nomenclature.Id} в кол-ве: {inventoryItemModel.Count}, недостаточно остатков на складе, остаток {departureWarehouseInventoryItem.Count}";
                        return response;
                    }
                }

                nomenclatureIds.Add(nomenclature.Id);

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
