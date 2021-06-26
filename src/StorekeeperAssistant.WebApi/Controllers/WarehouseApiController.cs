using Microsoft.AspNetCore.Mvc;
using StorekeeperAssistant.Api.Models.Warehouses;
using StorekeeperAssistant.BL.Services.Warehouses;
using System.Threading.Tasks;

namespace StorekeeperAssistant.WebApi.Controllers
{
    [Route("api/warehouses")]
    public class WarehouseApiController : ControllerBase
    {
        private readonly IWarehouseService _warehouseService;

        public WarehouseApiController(IWarehouseService warehouseService)
        {
            _warehouseService = warehouseService;
        }

        [HttpGet("get")]
        public async Task<GetWarehouseResponse> GetAsync(GetWarehouseRequest request)
            => await _warehouseService.GetAsync(request);
    }
}
