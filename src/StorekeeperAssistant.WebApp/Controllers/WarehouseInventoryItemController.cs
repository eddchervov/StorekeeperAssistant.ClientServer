using Microsoft.AspNetCore.Mvc;
using StorekeeperAssistant.Api.Models.WarehouseInventoryItems;
using StorekeeperAssistant.Api.Services;
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
        public async Task<IActionResult> GetWarehouseInventoryItemByWarehouseIdAsync(GetWarehouseInventoryItemRequest request)
        {
            var response = await _warehouseInventoryItemRemoteCallService.GetAsync(request);

            return Json(response);
        }
    }
}
