using StorekeeperAssistant.Api.Models.Warehouses;
using StorekeeperAssistant.DAL.Entities;
using StorekeeperAssistant.DAL.Repositories;
using System.Linq;
using System.Threading.Tasks;

namespace StorekeeperAssistant.BL.Services.Warehouses.Implementation
{
    internal class WarehouseService : IWarehouseService
    {
        private readonly IWarehouseRepository _warehouseRepository;

        public WarehouseService(IWarehouseRepository warehouseRepository)
        {
            _warehouseRepository = warehouseRepository;
        }

        public async Task<GetWarehouseResponse> GetAsync(GetWarehouseRequest request)
        {
            var response = new GetWarehouseResponse();

            var warehouses = await _warehouseRepository.GetAsync();

            response.Warehouses = warehouses.Select(MapToDTO);
            return response;
        }

        #region private
        private WarehouseDTO MapToDTO(Warehouse warehouse)
        {
            return new WarehouseDTO
            {
                Id = warehouse.Id,
                Name = warehouse.Name
            };
        }
        #endregion
    }
}
