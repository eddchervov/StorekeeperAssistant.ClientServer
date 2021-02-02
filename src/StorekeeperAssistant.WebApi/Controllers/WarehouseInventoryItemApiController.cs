using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StorekeeperAssistant.Api.Models.InventoryItem;
using StorekeeperAssistant.Api.Models.WarehouseInventoryItem;
using StorekeeperAssistant.BL.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StorekeeperAssistant.WebApi.Controllers
{
    [Route("api/warehouse-inventory-item")]
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

        [HttpGet("get-by-warehouse-id")]
        public async Task<GetWarehouseInventoryItemByWarehouseIdResponse> GetWarehouseInventoryItemByWarehouseIdAsync(GetWarehouseInventoryItemByWarehouseIdRequest request)
        {
            var response = new GetWarehouseInventoryItemByWarehouseIdResponse { IsSuccess = true, Message = string.Empty };

            var validationResponse = await _validationWarehouseInventoryItemService.ValidationWarehouseInventoryItemByWarehouseIdAsync(request);
            if (validationResponse.IsSuccess)
            {
                response = await _warehouseInventoryItemService.GetWarehouseInventoryItemByWarehouseIdAsync(request);
            }
            else
            {
                response.IsSuccess = false;
                response.Message = validationResponse.Message;
            }

            return response;
        }
    }
}
