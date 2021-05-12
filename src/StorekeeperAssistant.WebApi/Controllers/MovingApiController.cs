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
    [Route("api/movings")]
    public class MovingApiController : ControllerBase
    {
        private readonly IMovingService _movingService;
        private readonly IMovingCreationService _movingCreationService;
        private readonly IValidationMovingService _validationMovingService;

        public MovingApiController(IMovingService movingService,
            IMovingCreationService movingCreationService,
            IValidationMovingService validationMovingService)
        {
            _movingService = movingService;
            _movingCreationService = movingCreationService;
            _validationMovingService = validationMovingService;
        }

        [HttpGet("get")]
        public async Task<GetMovingResponse> GetMovingsAsync(GetMovingRequest request)
        {
            var response = await _movingService.GetMovingsAsync(request);

            return response;
        }

        [HttpPost("create")]
        public async Task<CreateMovingResponse> CreateMovingAsync([FromBody] CreateMovingRequest request)
        {
            var response = new CreateMovingResponse { IsSuccess = true, Message = string.Empty };

            var validationResponse = await _validationMovingService.ValidationCreateMovingAsync(request);
            if (validationResponse.IsSuccess)
            {
                response = await _movingCreationService.CreateAsync(request);
            }
            else
            {
                response.IsSuccess = false;
                response.Message = validationResponse.Message;
            }

            return response;
        }

        [HttpPut("delete")]
        public async Task<DeleteMovingResponse> DeleteMovingAsync([FromBody] DeleteMovingRequest request)
        {
            var response = await _movingService.DeleteMovingAsync(request);

            return response;
        }
    }
}
