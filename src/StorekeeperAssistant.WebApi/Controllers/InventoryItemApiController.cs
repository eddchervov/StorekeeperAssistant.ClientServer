using Microsoft.AspNetCore.Mvc;
using StorekeeperAssistant.Api.Models.InventoryItems;
using StorekeeperAssistant.BL.Services.InventoryItems;
using System.Threading.Tasks;

namespace StorekeeperAssistant.WebApi.Controllers
{
    [Route("api/inventory-items")]
    public class InventoryItemApiController : ControllerBase
    {
        private readonly IInventoryItemService _inventoryItemService;

        public InventoryItemApiController(IInventoryItemService inventoryItemService)
        {
            _inventoryItemService = inventoryItemService;
        }

        [HttpGet("get")]
        public async Task<GetInventoryItemsResponse> GetAsync(GetInventoryItemsRequest request) 
            => await _inventoryItemService.GetAsync(request);
    }
}
