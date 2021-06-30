using StorekeeperAssistant.Api.Models.InventoryItems;
using StorekeeperAssistant.Api.Models.Movings;
using StorekeeperAssistant.DAL.Entities;
using StorekeeperAssistant.DAL.Repositories;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace StorekeeperAssistant.BL.Services.Movings.Implementation
{
    internal class MovingService : IMovingService
    {
        private readonly IValidationMovingService _validationMovingService;
        private readonly ICreatorMovingService _creatorMovingService;
        private readonly IGetterMovingService _getterMovingService;
        private readonly IMovingRepository _movingRepository;

        public MovingService(IValidationMovingService validationMovingService,
            ICreatorMovingService creatorMovingService,
            IGetterMovingService getterMovingService,
            IMovingRepository movingRepository)
        {
            _validationMovingService = validationMovingService;
            _creatorMovingService = creatorMovingService;
            _getterMovingService = getterMovingService;
            _movingRepository = movingRepository;
        }

        public async Task<GetMovingResponse> GetAsync(GetMovingRequest request)
        {
            var response = new GetMovingResponse { IsSuccess = true, Message = string.Empty };

            var validationResponse = _validationMovingService.Validation(request);
            if (validationResponse.IsSuccess)
            {
                return await _getterMovingService.GetAsync(request);
            }

            response.IsSuccess = false;
            response.Message = validationResponse.Message;

            return response;
        }

        public async Task<CreateMovingResponse> CreateAsync(CreateMovingRequest request)
        {
            var response = new CreateMovingResponse { IsSuccess = true, Message = string.Empty };

            var validationResponse = await _validationMovingService.ValidationAsync(request);
            if (validationResponse.IsSuccess)
            {
                try
                {
                    await _creatorMovingService.CreateAsync(request);
                    return response;
                }
                catch (Exception ex)
                {
                    response.Message = ex.ToString();
                }
            }
            else response.Message = validationResponse.Message;

            response.IsSuccess = false;
            return response;
        }

        public async Task<DeleteMovingResponse> DeleteAsync(DeleteMovingRequest request)
        {
            var response = new DeleteMovingResponse { IsSuccess = true, Message = string.Empty };

            var moving = await _movingRepository.GetWithMovingDetailsByIdAsync(request.MovingId);

            if (moving == null) return _validationMovingService.ErrorResponse(request.MovingId);

            var createMovingRequest = CreateMovingRequest(moving);

            var responseCreateMoving = await CreateAsync(createMovingRequest);
            if (responseCreateMoving.IsSuccess == false)
            {
                response.IsSuccess = false;
                response.Message = responseCreateMoving.Message;
                return response;
            }

            moving.IsDeleted = true;
            await _movingRepository.UpdateAsync(moving);

            return response;
        }

        private CreateMovingRequest CreateMovingRequest(Moving moving)
        {
            return new CreateMovingRequest
            {
                // свап (для отката остатков)
                ArrivalWarehouseId = moving.DepartureWarehouseId,
                DepartureWarehouseId = moving.ArrivalWarehouseId,
                IsDeleted = true,
                InventoryItems = moving.MovingDetails.Select(x => new CreateInventoryItemDTO
                {
                    Id = x.InventoryItemId,
                    Count = x.Count
                }).ToList()
            };
        }
    }
}
