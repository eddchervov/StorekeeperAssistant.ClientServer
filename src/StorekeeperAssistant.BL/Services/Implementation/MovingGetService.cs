using StorekeeperAssistant.Api.Models.Moving;
using StorekeeperAssistant.Api.Models.MovingDetail;
using StorekeeperAssistant.Api.Models.Nomenclature;
using StorekeeperAssistant.Api.Models.Warehouse;
using StorekeeperAssistant.DAL.Entities;
using StorekeeperAssistant.DAL.Repositories;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StorekeeperAssistant.BL.Services.Implementation
{
    internal class MovingGetService : IMovingGetService
    {
        private readonly IMovingRepository _movingRepository;
        private readonly IMovingDetailRepository _movingDetailRepository;
        private readonly INomenclatureRepository _nomenclatureRepository;
        private readonly IWarehouseRepository _warehouseRepository;

        private List<Warehouse> _warehouses;
        private List<MovingDetail> _movingDetails;

        public MovingGetService(IMovingRepository movingRepository,
            IMovingDetailRepository movingDetailRepository,
            INomenclatureRepository nomenclatureRepository,
            IWarehouseRepository warehouseRepository)
        {
            _movingRepository = movingRepository;
            _movingDetailRepository = movingDetailRepository;
            _nomenclatureRepository = nomenclatureRepository;
            _warehouseRepository = warehouseRepository;
        }

        public async Task<GetMovingResponse> GetMovingsAsync(GetMovingRequest request)
        {
            var response = new GetMovingResponse { IsSuccess = true, Message = string.Empty };

            var responseDAL = await _movingRepository.GetIsActiveMovingsAsync(request.SkipCount, request.TakeCount);

            await InitializationListsAsync(responseDAL.Movings);

            var movingModels = new List<MovingModel>();
            foreach (var moving in responseDAL.Movings) movingModels.Add(await ConvertModelAsync(moving));

            response.TotalCount = responseDAL.TotalCount;
            response.MovingModels = movingModels;
            return response;
        }

        private async Task InitializationListsAsync(List<Moving> movings)
        {
            var arrivalWarehouseIds = movings.Where(x => x.ArrivalWarehouseId != null).Select(x => x.ArrivalWarehouseId.Value);
            var departureWarehouseIds = movings.Where(x => x.DepartureWarehouseId != null).Select(x => x.DepartureWarehouseId.Value);
            var warehouseIds = arrivalWarehouseIds.Union(departureWarehouseIds);
            await GetWarehousesAsync(warehouseIds);

            var movingIds = movings.Select(x => x.Id);
            await GetMovingDetailsAsync(movingIds);


        }

        private async Task GetWarehousesAsync(IEnumerable<int> warehouseIds)
        {
            _warehouses = await _warehouseRepository.GetByIdsAsync(warehouseIds);
        }

        private async Task GetMovingDetailsAsync(IEnumerable<int> movingIds)
        {
            _movingDetails = await _movingDetailRepository.GetByMovingIdsAsync(movingIds);
        }

        private async Task<MovingModel> ConvertModelAsync(Moving moving)
        {
            WarehouseModel arrivalWarehouse = null;
            if (moving.ArrivalWarehouseId != null)
                arrivalWarehouse = ConvertModel(moving.ArrivalWarehouseId.Value);

            WarehouseModel departureWarehouse = null;
            if (moving.DepartureWarehouseId != null)
                departureWarehouse = ConvertModel(moving.DepartureWarehouseId.Value);

            var movingDetailModels = new List<MovingDetailModel>();
            var movingDetails = _movingDetails.Where(x=>x.MovingId == moving.Id);

            var nomenclatureIds = movingDetails.Select(x => x.NomenclatureId);
            var nomenclatures = await _nomenclatureRepository.GetByIdsAsync(nomenclatureIds);

            foreach (var movingDetail in movingDetails)
            {
                var nomenclature = nomenclatures.FirstOrDefault(x=>x.Id == movingDetail.NomenclatureId);

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

        private WarehouseModel ConvertModel(int warehouseId)
        {
            var warehouse = _warehouses.FirstOrDefault(x => x.Id == warehouseId);
            return new WarehouseModel
            {
                Id = warehouse.Id,
                Name = warehouse.Name
            };
        }
    }
}
