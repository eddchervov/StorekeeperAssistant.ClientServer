using Microsoft.AspNetCore.Mvc;
using StorekeeperAssistant.Api.Models.Warehouses;
using StorekeeperAssistant.Api.Services;
using System.Threading.Tasks;

namespace StorekeeperAssistant.WebApp.Controllers
{
    public class WarehouseController : Controller
    {
        private readonly IWarehouseRemoteCallService _warehouseRemoteCallService;

        public WarehouseController(IWarehouseRemoteCallService warehouseRemoteCallService)
        {
            _warehouseRemoteCallService = warehouseRemoteCallService;
        }

        [HttpGet("warehouses/get")]
        public async Task<IActionResult> GetAsync(GetWarehouseRequest request)
        {
            var response = await _warehouseRemoteCallService.GetAsync(request);

            return Json(response);
        }
    }
}
