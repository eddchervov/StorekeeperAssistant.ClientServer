using Microsoft.AspNetCore.Mvc;
using StorekeeperAssistant.Api.Models.Nomenclature;
using StorekeeperAssistant.Api.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StorekeeperAssistant.WebApp.Controllers
{
    public class NomenclatureController : Controller
    {
        private readonly INomenclatureRemoteCallService _nomenclatureRemoteCallService;

        public NomenclatureController(INomenclatureRemoteCallService nomenclatureRemoteCallService)
        {
            _nomenclatureRemoteCallService = nomenclatureRemoteCallService;
        }

        [HttpGet("nomenclatures/get")]
        public async Task<IActionResult> GetNomenclaturesAsync(GetNomenclaturesRequest request)
        {
            var response = await _nomenclatureRemoteCallService.GetNomenclaturesAsync(request);

            return Json(response);
        }
    }
}
