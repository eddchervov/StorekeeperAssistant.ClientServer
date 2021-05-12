using StorekeeperAssistant.Api.Models.Moving;
using StorekeeperAssistant.DAL.DBContext;
using StorekeeperAssistant.DAL.Repositories;
using System;
using System.Threading.Tasks;

namespace StorekeeperAssistant.BL.Services.Implementation
{
    internal class MovingCreationService : IMovingCreationService
    {
        private readonly IMovingRepository _movingRepository;
        private readonly IWarehouseInventoryItemRepository _warehouseInventoryItemRepository;
        private readonly IMovingDetailRepository _movingDetailRepository;
        private readonly IAppDBContext _appDBContext;

        public MovingCreationService(IMovingRepository movingRepository,
            IWarehouseInventoryItemRepository warehouseInventoryItemRepository,
            IMovingDetailRepository movingDetailRepository,
            IAppDBContext appDBContext)
        {
            _movingRepository = movingRepository;
            _warehouseInventoryItemRepository = warehouseInventoryItemRepository;
            _movingDetailRepository = movingDetailRepository;
            _appDBContext = appDBContext;
        }

        public async Task<CreateMovingResponse> CreateAsync(CreateMovingRequest request)
        {
            var response = new CreateMovingResponse { IsSuccess = true, Message = string.Empty };
            var utcNow = DateTime.UtcNow;

            try
            {
                using var transaction = _appDBContext.BeginTransaction();

                var moving = MovingCreationHelperService.CreateMoving(request.DepartureWarehouseId, request.ArrivalWarehouseId, utcNow);
                await _movingRepository.InsertAsync(moving);

                foreach (var nomenclature in request.CreateInventoryItemModels)
                {
                    var movingDetail = MovingCreationHelperService.CreateMovingDetail(moving.Id, nomenclature.Id, nomenclature.Count);
                    await _movingDetailRepository.InsertAsync(movingDetail);

                    if (request.DepartureWarehouseId != null)
                    {
                        var departureWarehouseInventoryItem = await _warehouseInventoryItemRepository.GetLastAsync(request.DepartureWarehouseId.Value, nomenclature.Id);
                        var newCountDeparture = departureWarehouseInventoryItem.Count - nomenclature.Count;

                        var warehouseInventoryItem = MovingCreationHelperService.CreateWarehouseInventoryItem(nomenclature.Id, utcNow, newCountDeparture, request.DepartureWarehouseId.Value, moving.Id);
                        await _warehouseInventoryItemRepository.InsertAsync(warehouseInventoryItem);
                    }

                    if (request.ArrivalWarehouseId != null)
                    {
                        var newCountArrival = nomenclature.Count;
                        var arrivalWarehouseInventoryItem = await _warehouseInventoryItemRepository.GetLastAsync(request.ArrivalWarehouseId.Value, nomenclature.Id);
                        if (arrivalWarehouseInventoryItem != null) newCountArrival += arrivalWarehouseInventoryItem.Count;

                        var warehouseInventoryItem = MovingCreationHelperService.CreateWarehouseInventoryItem(nomenclature.Id, utcNow, newCountArrival, request.DepartureWarehouseId.Value, moving.Id);
                        await _warehouseInventoryItemRepository.InsertAsync(warehouseInventoryItem);
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
    }
}
