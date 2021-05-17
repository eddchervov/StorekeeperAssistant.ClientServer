using Microsoft.AspNetCore.Mvc;
using StorekeeperAssistant.Api.Models.Moving;
using StorekeeperAssistant.BL.Services;
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
        public async Task<GetMovingResponse> GetMovingsAsync(GetMovingRequest request)
        {
            return await _movingService.GetMovingsAsync(request);
        }

        [HttpPost("create")]
        public async Task<CreateMovingResponse> CreateMovingAsync([FromBody] CreateMovingRequest request)
        {
            return await _movingService.CreateMovingAsync(request);
        }

        [HttpPut("delete")]
        public async Task<DeleteMovingResponse> DeleteMovingAsync([FromBody] DeleteMovingRequest request)
        {
            return await _movingService.DeleteMovingAsync(request);
        }
    }
}
