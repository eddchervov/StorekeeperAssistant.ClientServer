using Microsoft.AspNetCore.Mvc;
using StorekeeperAssistant.Api.Models.InventoryItems;
using StorekeeperAssistant.Api.Services;
using System.Threading.Tasks;

namespace StorekeeperAssistant.WebApp.Controllers
{
    public class NomenclatureController : Controller
    {
        private readonly IInventoryItemRemoteCallService _nomenclatureRemoteCallService;

        public NomenclatureController(IInventoryItemRemoteCallService nomenclatureRemoteCallService)
        {
            _nomenclatureRemoteCallService = nomenclatureRemoteCallService;
        }

        [HttpGet("nomenclatures/get")]
        public async Task<IActionResult> GetNomenclaturesAsync(GetInventoryItemsRequest request)
        {
            var response = await _nomenclatureRemoteCallService.GetAsync(request);

            return Json(response);
        }
    }
}
