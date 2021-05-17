using StorekeeperAssistant.Api.Models.Warehouse;
using StorekeeperAssistant.DAL.Entities;
using StorekeeperAssistant.DAL.Repositories;
using System.Linq;
using System.Threading.Tasks;

namespace StorekeeperAssistant.BL.Services.Implementation
{
    internal class WarehouseService : IWarehouseService
    {
        private readonly IWarehouseRepository _warehouseRepository;

        public WarehouseService(IWarehouseRepository warehouseRepository)
        {
            _warehouseRepository = warehouseRepository;
        }

        public async Task<GetWarehouseResponse> GetWarehousesAsync(GetWarehouseRequest request)
        {
            var response = new GetWarehouseResponse { };

            var warehouses = await _warehouseRepository.GetListAsync();

            response.WarehouseModels = warehouses.Select(ConvertModel).ToList();
            return response;
        }

        private WarehouseModel ConvertModel(Warehouse warehouse)
        {
            return new WarehouseModel
            {
                Id = warehouse.Id,
                Name = warehouse.Name
            };
        }
    }
}
