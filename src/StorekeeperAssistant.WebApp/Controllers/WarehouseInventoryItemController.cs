using Microsoft.AspNetCore.Mvc;
using StorekeeperAssistant.Api.Models.InventoryItem;
using StorekeeperAssistant.Api.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StorekeeperAssistant.WebApp.Controllers
{
    public class WarehouseInventoryItemController : Controller
    {
        private readonly IWarehouseInventoryItemRemoteCallService _warehouseInventoryItemRemoteCallService;

        public WarehouseInventoryItemController(IWarehouseInventoryItemRemoteCallService warehouseInventoryItemRemoteCallService)
        {
            _warehouseInventoryItemRemoteCallService = warehouseInventoryItemRemoteCallService;
        }

        [HttpGet("warehouse-inventory-item/report")]
        public IActionResult Report()
        {
            return View();
        } 

        [HttpGet("warehouse-inventory-item/get-by-warehouse-id/{WarehouseId:int}")]
        public async Task<IActionResult> GetWarehouseInventoryItemByWarehouseIdAsync(GetWarehouseInventoryItemByWarehouseIdRequest request)
        {
            var response = await _warehouseInventoryItemRemoteCallService.GetWarehouseInventoryItemByWarehouseIdAsync(request);

            return Json(response);
        }

        
    }
}
