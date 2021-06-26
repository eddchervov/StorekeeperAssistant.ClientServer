using Microsoft.AspNetCore.Mvc;
using StorekeeperAssistant.Api.Models.Movings;
using StorekeeperAssistant.BL.Services.Movings;
using System.Threading.Tasks;

namespace StorekeeperAssistant.WebApi.Controllers
{
    [Route("api/movings")]
    public class MovingApiController : ControllerBase
    {
        private readonly IMovingService _movingService;

        public MovingApiController(IMovingService movingService)
        {
            _movingService = movingService;
        }

        [HttpGet("get")]
        public async Task<GetMovingResponse> GetAsync(GetMovingRequest request) 
            => await _movingService.GetAsync(request);

        [HttpPost("create")]
        public async Task<CreateMovingResponse> CreateAsync([FromBody] CreateMovingRequest request) 
            => await _movingService.CreateAsync(request);

        [HttpPut("delete")]
        public async Task<DeleteMovingResponse> DeleteAsync([FromBody] DeleteMovingRequest request) 
            => await _movingService.DeleteAsync(request);
    }
}
