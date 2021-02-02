using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StorekeeperAssistant.Api.Models.Nomenclature;
using StorekeeperAssistant.BL.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StorekeeperAssistant.WebApi.Controllers
{
    [Route("api/nomenclature")]
    public class NomenclatureApiController : ControllerBase
    {
        private readonly INomenclatureService _nomenclatureService;

        public NomenclatureApiController(INomenclatureService nomenclatureService)
        {
            _nomenclatureService = nomenclatureService;
        }

        [HttpGet("get")]
        public async Task<GetNomenclaturesResponse> GetNomenclaturesAsync(GetNomenclaturesRequest request)
        {
            var response = await _nomenclatureService.GetNomenclaturesAsync(request);
            return response;
        }
    }
}
