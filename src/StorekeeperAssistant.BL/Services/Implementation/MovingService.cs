using StorekeeperAssistant.Api.Models.InventoryItem;
using StorekeeperAssistant.Api.Models.Moving;
using StorekeeperAssistant.Api.Models.Nomenclature;
using StorekeeperAssistant.Api.Models.Warehouse;
using StorekeeperAssistant.DAL.Entities;
using StorekeeperAssistant.DAL.Models;
using StorekeeperAssistant.DAL.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StorekeeperAssistant.BL.Services.Implementation
{
    internal class MovingService : IMovingService
    {
        private readonly IMovingRepository _movingRepository;
        private readonly IWarehouseRepository _warehouseRepository;
        private readonly IMovingInventoryItemsRepository _movingInventoryItemsRepository;

        public MovingService(IMovingRepository movingRepository,
            IWarehouseRepository warehouseRepository,
            IMovingInventoryItemsRepository movingInventoryItemsRepository)
        {
            _movingRepository = movingRepository;
            _warehouseRepository = warehouseRepository;
            _movingInventoryItemsRepository = movingInventoryItemsRepository;
        }

        public async Task<GetMovingResponse> GetMovingsAsync(GetMovingRequest request)
        {
            var response = new GetMovingResponse { IsSuccess = true, Message = string.Empty };

            var responseDAL = await _movingRepository.GetIsActiveMovingsAsync(request.SkipCount, request.TakeCount);

            var movingModels = new List<MovingModel>();
            foreach (var moving in responseDAL.Movings) movingModels.Add(await ConvertModelAsync(moving));

            response.TotalCount = responseDAL.TotalCount;
            response.MovingModels = movingModels;
            return response;
        }

        private async Task<MovingModel> ConvertModelAsync(Moving moving)
        {
            var inventoryItemsDALModels = await _movingInventoryItemsRepository.GetInventoryItemsByMovingIdAsync(moving.Id);

            var movingModelm = new MovingModel
            {
                Id = moving.Id,
                TypeTransaction = moving.TypeTransaction,
                DateTime = moving.DateTime,
                ArrivalWarehouse = await ConvertModelAsync(moving.ArrivalWarehouseId),
                DepartureWarehouse = await ConvertModelAsync(moving.ArrivalWarehouseId),
                InventoryItemModels = inventoryItemsDALModels.Select(ConvertModel).ToList()
            };

            return movingModelm;
        }

        private InventoryItemModel ConvertModel(InventoryItemsDALModel inventoryItemsDALModel)
        {
            return new InventoryItemModel
            {
                Id = inventoryItemsDALModel.Id,
                NomenclatureModel = new NomenclatureModel
                {
                    Id = inventoryItemsDALModel.NomenclatureDALModel.Id,
                    Name = inventoryItemsDALModel.NomenclatureDALModel.Name
                }
            };
        }

        private async Task<WarehouseModel> ConvertModelAsync(int warehouseId)
        {
            var warehouse = await _warehouseRepository.GetByIdAsync(warehouseId);
            return new WarehouseModel
            {
                Id = warehouse.Id,
                Name = warehouse.Name
            };
        }
    }
}
