using Microsoft.AspNetCore.Mvc;
using StorekeeperAssistant.Api.Models.InventoryItems;
using StorekeeperAssistant.Api.Services;
using System.Threading.Tasks;

namespace StorekeeperAssistant.WebApp.Controllers
{
    public class InventoryItemController : Controller
    {
        private readonly IInventoryItemRemoteCallService _inventoryItemRemoteCallService;

        public InventoryItemController(IInventoryItemRemoteCallService inventoryItemRemoteCallService)
        {
            _inventoryItemRemoteCallService = inventoryItemRemoteCallService;
        }

        [HttpGet("inventory-items/get")]
        public async Task<IActionResult> GetAsync(GetInventoryItemsRequest request)
        {
            var response = await _inventoryItemRemoteCallService.GetAsync(request);

            return Json(response);
        }
    }
}
