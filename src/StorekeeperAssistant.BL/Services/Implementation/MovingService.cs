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

        public async Task<CreateMovingResponse> CreateMovingAsync(CreateMovingRequest request)
        {
            var response = new CreateMovingResponse { IsSuccess = true, Message = string.Empty };
            var utcNow = DateTime.UtcNow;

            try
            {
                using var transaction = _appDBContext.BeginTransaction();

                Warehouse departureWarehouse = null;
                if (request.DepartureWarehouseId != null)
                    departureWarehouse = await _warehouseRepository.GetByIdAsync(request.DepartureWarehouseId.Value);

                Warehouse arrivalWarehouse = null;
                if (request.ArrivalWarehouseId != null)
                    arrivalWarehouse = await _warehouseRepository.GetByIdAsync(request.ArrivalWarehouseId.Value);

                var moving = new Moving
                {
                    DepartureWarehouseId = departureWarehouse?.Id,
                    ArrivalWarehouseId = arrivalWarehouse?.Id,
                    IsActive = true,
                    DateTime = utcNow
                };

                await _movingRepository.InsertAsync(moving);

                foreach (var ii in request.CreateInventoryItemModels)
                {
                    var nomenclature = await _nomenclatureRepository.GetByIdAsync(ii.Id);

                    var movingDetail = new MovingDetail
                    {
                        MovingId = moving.Id,
                        NomenclatureId = nomenclature.Id,
                        Count = ii.Count
                    };

                    await _movingDetailRepository.InsertAsync(movingDetail);

                    // Расход
                    if (departureWarehouse != null)
                    {
                        var departureWarehouseInventoryItem = await _warehouseInventoryItemRepository.GetLastByWarehouseIdAndNomenclatureIdAsync(departureWarehouse.Id, nomenclature.Id);
                        var newCountDeparture = departureWarehouseInventoryItem.Count - ii.Count;

                        var newDepartureWarehouseInventoryItem = new WarehouseInventoryItem
                        {
                            NomenclatureId = nomenclature.Id,
                            DateTime = utcNow,
                            Count = newCountDeparture,
                            WarehouseId = departureWarehouse.Id,
                            MovingId = moving.Id,
                            IsActive = true
                        };
                        await _warehouseInventoryItemRepository.InsertAsync(newDepartureWarehouseInventoryItem);
                    }

                    // Приход
                    if (arrivalWarehouse != null)
                    {
                        var newCountArrival = ii.Count;
                        var arrivalWarehouseInventoryItem = await _warehouseInventoryItemRepository.GetLastByWarehouseIdAndNomenclatureIdAsync(arrivalWarehouse.Id, nomenclature.Id);
                        if (arrivalWarehouseInventoryItem != null)
                            newCountArrival = arrivalWarehouseInventoryItem.Count + ii.Count;

                        var newArrivalWarehouseInventoryItem = new WarehouseInventoryItem
                        {
                            NomenclatureId = nomenclature.Id,
                            DateTime = utcNow,
                            Count = newCountArrival,
                            WarehouseId = arrivalWarehouse.Id,
                            MovingId = moving.Id,
                            IsActive = true
                        };
                        await _warehouseInventoryItemRepository.InsertAsync(newArrivalWarehouseInventoryItem);
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

        public async Task<DeleteMovingResponse> DeleteMovingAsync(DeleteMovingRequest request)
        {
            var response = new DeleteMovingResponse { IsSuccess = true, Message = string.Empty };

            try
            {
                using var transaction = _appDBContext.BeginTransaction();

                var moving = await _movingRepository.GetByIdAsync(request.MovingId);

                Warehouse departureWarehouse = null;
                if (moving.DepartureWarehouseId != null)
                    departureWarehouse = await _warehouseRepository.GetByIdAsync(moving.DepartureWarehouseId.Value);

                Warehouse arrivalWarehouse = null;
                if (moving.ArrivalWarehouseId != null)
                    arrivalWarehouse = await _warehouseRepository.GetByIdAsync(moving.ArrivalWarehouseId.Value);

                moving.IsActive = false;

                await _movingRepository.UpdateAsync(moving);

                var movingDetails = await _movingDetailRepository.GetByMovingIdAsync(moving.Id);

                foreach (var movingDetail in movingDetails)
                {
                    var nomenclature = await _nomenclatureRepository.GetByIdAsync(movingDetail.NomenclatureId);

                    WarehouseInventoryItem departureWarehouseInventoryItem = null;
                    if (departureWarehouse != null)
                    {
                        departureWarehouseInventoryItem = await _warehouseInventoryItemRepository.GetByMovingIdAsync(moving.Id, departureWarehouse.Id, nomenclature.Id);
                        departureWarehouseInventoryItem.IsActive = false;
                        departureWarehouseInventoryItem.Count = departureWarehouseInventoryItem.Count - movingDetail.Count;
                        await _warehouseInventoryItemRepository.UpdateAsync(departureWarehouseInventoryItem);

                        var editWarehouseInventoryItems = await _warehouseInventoryItemRepository.GetByPeriodAsync(departureWarehouse.Id, nomenclature.Id, departureWarehouseInventoryItem.DateTime, DateTime.UtcNow);
                        foreach (var editWarehouseInventoryItem in editWarehouseInventoryItems)
                        {
                            editWarehouseInventoryItem.Count = editWarehouseInventoryItem.Count + movingDetail.Count;
                            if (editWarehouseInventoryItem.Count < 1)
                            {
                                editWarehouseInventoryItem.IsActive = false;
                            }
                            await _warehouseInventoryItemRepository.UpdateAsync(editWarehouseInventoryItem);
                        }
                    }

                    WarehouseInventoryItem arrivalWarehouseInventoryItem = null;
                    if (arrivalWarehouse != null)
                    {
                        arrivalWarehouseInventoryItem = await _warehouseInventoryItemRepository.GetByMovingIdAsync(moving.Id, arrivalWarehouse.Id, nomenclature.Id);
                        arrivalWarehouseInventoryItem.Count = arrivalWarehouseInventoryItem.Count - movingDetail.Count;
                        arrivalWarehouseInventoryItem.IsActive = false;

                        await _warehouseInventoryItemRepository.UpdateAsync(arrivalWarehouseInventoryItem);

                        var editWarehouseInventoryItems = await _warehouseInventoryItemRepository.GetByPeriodAsync(arrivalWarehouse.Id, nomenclature.Id, arrivalWarehouseInventoryItem.DateTime, DateTime.UtcNow);
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
