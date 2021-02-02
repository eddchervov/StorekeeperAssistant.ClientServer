using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StorekeeperAssistant.Api.Models.Warehouse;
using StorekeeperAssistant.BL.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StorekeeperAssistant.WebApi.Controllers
{
    [Route("api/warehouse")]
    public class WarehouseApiController : ControllerBase
    {
        private readonly IWarehouseService _warehouseService;

        public WarehouseApiController(IWarehouseService warehouseService)
        {
            _warehouseService = warehouseService;
        }

        [HttpGet("get")]
        public async Task<GetWarehouseResponse> GetWarehousesAsync(GetWarehouseRequest request)
        {
            var response = await _warehouseService.GetWarehousesAsync(request);

            return response;
        }
    }
}
