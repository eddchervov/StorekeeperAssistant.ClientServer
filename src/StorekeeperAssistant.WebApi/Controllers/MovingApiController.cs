using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StorekeeperAssistant.Api.Models.Moving;
using StorekeeperAssistant.BL.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StorekeeperAssistant.WebApi.Controllers
{
    [Route("api/moving")]
    [ApiController]
    public class MovingApiController : ControllerBase
    {
        private readonly IMovingService _movingService;

        public MovingApiController(IMovingService movingService)
        {
            _movingService = movingService;
        }

        [HttpGet("get")]
        public async Task<GetMovingResponse> GetMovingsAsync(GetMovingRequest request)
        {
            var response = await _movingService.GetMovingsAsync(request);

            return response;
        }
    }
}
