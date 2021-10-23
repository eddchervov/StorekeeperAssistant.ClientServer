using Microsoft.AspNetCore.Mvc;
using StorekeeperAssistant.Api.Models.WarehouseInventoryItems;
using StorekeeperAssistant.BL.Services.WarehouseInventoryItems;
using System.Threading.Tasks;

namespace StorekeeperAssistant.WebApi.Controllers
{
    [Route("api/warehouse-inventory-items")]
    public class WarehouseInventoryItemApiController : ControllerBase
    {
        private readonly IWarehouseInventoryItemService _warehouseInventoryItemService;
        private readonly IValidationWarehouseInventoryItemService _validationWarehouseInventoryItemService;

        public WarehouseInventoryItemApiController(IWarehouseInventoryItemService warehouseInventoryItemService,
            IValidationWarehouseInventoryItemService validationWarehouseInventoryItemService)
        {
            _warehouseInventoryItemService = warehouseInventoryItemService;
            _validationWarehouseInventoryItemService = validationWarehouseInventoryItemService;
        }

        [HttpPost("get")]
        public async Task<GetWarehouseInventoryItemResponse> GetAsync([FromBody]GetWarehouseInventoryItemRequest request)
        {
            var response = new GetWarehouseInventoryItemResponse { IsSuccess = true, Message = string.Empty };

            var validationResponse = await _validationWarehouseInventoryItemService.ValidationAsync(request);
            if (validationResponse.IsSuccess)
            {
                return await _warehouseInventoryItemService.GetAsync(request);
            }

            response.IsSuccess = false;
            response.Message = validationResponse.Message;

            return response;
        }
    }
}
