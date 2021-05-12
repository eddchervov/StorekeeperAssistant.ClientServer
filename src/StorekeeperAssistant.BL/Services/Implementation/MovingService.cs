using StorekeeperAssistant.Api.Models.Moving;
using StorekeeperAssistant.Api.Models.MovingDetail;
using StorekeeperAssistant.Api.Models.Nomenclature;
using StorekeeperAssistant.Api.Models.Warehouse;
using StorekeeperAssistant.DAL.DBContext;
using StorekeeperAssistant.DAL.Entities;
using StorekeeperAssistant.DAL.Repositories;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace StorekeeperAssistant.BL.Services.Implementation
{
    internal class MovingService : IMovingService
    {
        private readonly IMovingRepository _movingRepository;
        private readonly IWarehouseRepository _warehouseRepository;
        private readonly IWarehouseInventoryItemRepository _warehouseInventoryItemRepository;
        private readonly INomenclatureRepository _nomenclatureRepository;
        private readonly IMovingDetailRepository _movingDetailRepository;
        private readonly IAppDBContext _appDBContext;

        public MovingService(IMovingRepository movingRepository,
            IWarehouseRepository warehouseRepository,
            IWarehouseInventoryItemRepository warehouseInventoryItemRepository,
            INomenclatureRepository nomenclatureRepository,
            IMovingDetailRepository movingDetailRepository,
            IAppDBContext appDBContext)
        {
            _movingRepository = movingRepository;
            _warehouseRepository = warehouseRepository;
            _warehouseInventoryItemRepository = warehouseInventoryItemRepository;
            _nomenclatureRepository = nomenclatureRepository;
            _movingDetailRepository = movingDetailRepository;
            _appDBContext = appDBContext;
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

        private async Task<WarehouseInventoryItem> UpdateWarehouseInventoryItemAsync(int movingId, int warehouseId, int nomenclatureId, int movingDetailCount)
        {
            WarehouseInventoryItem warehouseInventoryItem = await _warehouseInventoryItemRepository.GetByMovingIdAsync(movingId, warehouseId, nomenclatureId);
            warehouseInventoryItem.IsActive = false;
            warehouseInventoryItem.Count -= movingDetailCount;

            await _warehouseInventoryItemRepository.UpdateAsync(warehouseInventoryItem);

            return warehouseInventoryItem;
        }

        public async Task<DeleteMovingResponse> DeleteMovingAsync(DeleteMovingRequest request)
        {
            var response = new DeleteMovingResponse { IsSuccess = true, Message = string.Empty };

            try
            {
                using var transaction = _appDBContext.BeginTransaction();

                var moving = await _movingRepository.GetByIdAsync(request.MovingId);
                moving.IsActive = false;

                await _movingRepository.UpdateAsync(moving);

                var movingDetails = await _movingDetailRepository.GetByMovingIdAsync(moving.Id);

                foreach (var movingDetail in movingDetails)
                {
                    var nomenclature = await _nomenclatureRepository.GetByIdAsync(movingDetail.NomenclatureId);

                    if (moving.DepartureWarehouseId != null)
                    {
                        var departureWarehouseInventoryItem = await _warehouseInventoryItemRepository.GetByMovingIdAsync(moving.Id, moving.DepartureWarehouseId.Value, nomenclature.Id);
                        departureWarehouseInventoryItem.IsActive = false;
                        departureWarehouseInventoryItem.Count -= movingDetail.Count;

                        await _warehouseInventoryItemRepository.UpdateAsync(departureWarehouseInventoryItem);

                        var editWarehouseInventoryItems = await _warehouseInventoryItemRepository.GetByPeriodAsync(moving.DepartureWarehouseId.Value, nomenclature.Id, departureWarehouseInventoryItem.DateTime, DateTime.UtcNow);
                        foreach (var editWarehouseInventoryItem in editWarehouseInventoryItems)
                        {
                            editWarehouseInventoryItem.Count += movingDetail.Count;
                            if (editWarehouseInventoryItem.Count < 1)
                            {
                                editWarehouseInventoryItem.IsActive = false;
                            }
                            await _warehouseInventoryItemRepository.UpdateAsync(editWarehouseInventoryItem);
                        }
                    }

                    if (moving.ArrivalWarehouseId != null)
                    {
                        var arrivalWarehouseInventoryItem = await UpdateWarehouseInventoryItemAsync(moving.Id, moving.ArrivalWarehouseId.Value, nomenclature.Id, movingDetail.Count);

                        var editWarehouseInventoryItems = await _warehouseInventoryItemRepository.GetByPeriodAsync(moving.ArrivalWarehouseId.Value, nomenclature.Id, arrivalWarehouseInventoryItem.DateTime, DateTime.UtcNow);
                        foreach (var editWarehouseInventoryItem in editWarehouseInventoryItems)
                        {
                            var count = editWarehouseInventoryItem.Count - movingDetail.Count;
                            if (count <= 0)
                            {
                                response.IsSuccess = false;
                                response.Message = $"Сначало удалите перемещения, на складе недостаточно товаров для удаления перемещения";
                                return response;
                            }

                            await _warehouseInventoryItemRepository.UpdateAsync(editWarehouseInventoryItem);
                        }
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

        private async Task<MovingModel> ConvertModelAsync(Moving moving)
        {
            WarehouseModel arrivalWarehouse = null;
            if (moving.ArrivalWarehouseId != null)
                arrivalWarehouse = await ConvertModelAsync(moving.ArrivalWarehouseId.Value);

            WarehouseModel departureWarehouse = null;
            if (moving.DepartureWarehouseId != null)
                departureWarehouse = await ConvertModelAsync(moving.DepartureWarehouseId.Value);

            var movingDetailModels = new List<MovingDetailModel>();
            var movingDetails = await _movingDetailRepository.GetByMovingIdAsync(moving.Id);
            foreach (var movingDetail in movingDetails)
            {
                var nomenclature = await _nomenclatureRepository.GetByIdAsync(movingDetail.NomenclatureId);

                var movingDetailModel = new MovingDetailModel
                {
                    Id = movingDetail.Id,
                    Count = movingDetail.Count,
                    NomenclatureModel = new NomenclatureModel
                    {
                        Id = nomenclature.Id,
                        Name = nomenclature.Name
                    }
                };

                movingDetailModels.Add(movingDetailModel);
            }

            var movingModel = new MovingModel
            {
                Id = moving.Id,
                DateTime = moving.DateTime,
                ArrivalWarehouse = arrivalWarehouse,
                DepartureWarehouse = departureWarehouse,
                MovingDetailModels = movingDetailModels
            };

            return movingModel;
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
