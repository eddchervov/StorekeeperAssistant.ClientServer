using StorekeeperAssistant.Api.Models;
using StorekeeperAssistant.Api.Models.InventoryItem;
using StorekeeperAssistant.DAL.Repositories;
using System.Threading.Tasks;

namespace StorekeeperAssistant.BL.Services.Implementation
{
    internal class ValidationWarehouseInventoryItemService : IValidationWarehouseInventoryItemService
    {
        private readonly IWarehouseRepository _warehouseRepository;

        public ValidationWarehouseInventoryItemService(IWarehouseRepository warehouseRepository)
        {
            _warehouseRepository = warehouseRepository;
        }

        public async Task<BaseResponse> ValidationWarehouseInventoryItemByWarehouseIdAsync(GetWarehouseInventoryItemByWarehouseIdRequest request)
        {
            var response = new BaseResponse { IsSuccess = true, Message = string.Empty };

            var warehouse = await _warehouseRepository.GetByIdAsync(request.WarehouseId);
            if (warehouse == null)
            {
                response.IsSuccess = false;
                response.Message = $"Склада с id={request.WarehouseId} не найдено";
                return response;
            }

            return response;
        }
    }
}
