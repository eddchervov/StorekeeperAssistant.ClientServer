using Microsoft.AspNetCore.Mvc;
using StorekeeperAssistant.Api.Models.WarehouseInventoryItems;
using StorekeeperAssistant.Api.Services;
using System.Threading.Tasks;

namespace StorekeeperAssistant.Vue.WebApp.Controllers
{
    public class WarehouseInventoryItemController : Controller
    {
        private readonly IWarehouseInventoryItemRemoteCallService _warehouseInventoryItemRemoteCallService;

        public WarehouseInventoryItemController(IWarehouseInventoryItemRemoteCallService warehouseInventoryItemRemoteCallService)
        {
            _warehouseInventoryItemRemoteCallService = warehouseInventoryItemRemoteCallService;
        }

        [HttpGet("warehouse-inventory-items/report")]
        public IActionResult Report()
        {
            return View();
        } 

        [HttpGet("warehouse-inventory-items/get/{WarehouseId:int}")]
        public async Task<IActionResult> GetAsync(GetWarehouseInventoryItemRequest request)
        {
            var response = await _warehouseInventoryItemRemoteCallService.GetAsync(request);

            return Json(response);
        }
    }
}
