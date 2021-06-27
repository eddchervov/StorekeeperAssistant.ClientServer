using StorekeeperAssistant.Api.Models.InventoryItems;
using StorekeeperAssistant.Api.Models.Movings;
using StorekeeperAssistant.DAL.Entities;
using StorekeeperAssistant.DAL.Repositories;
using System.Linq;
using System.Threading.Tasks;

namespace StorekeeperAssistant.BL.Services.Movings.Implementation
{
    internal class MovingService : IMovingService
    {
        private readonly IValidationMovingService _validationMovingService;
        private readonly ICreatorMovingService _creatorMovingService;
        private readonly IGetterMovingService _getterMovingService;
        private readonly IMovingDetailRepository _movingDetailRepository;
        private readonly IMovingRepository _movingRepository;

        public MovingService(IValidationMovingService validationMovingService,
            ICreatorMovingService creatorMovingService,
            IGetterMovingService getterMovingService,
            IMovingDetailRepository movingDetailRepository,
            IMovingRepository movingRepository)
        {
            _validationMovingService = validationMovingService;
            _creatorMovingService = creatorMovingService;
            _getterMovingService = getterMovingService;
            _movingDetailRepository = movingDetailRepository;
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
                return await _creatorMovingService.CreateAsync(request);
            }

            response.IsSuccess = false;
            response.Message = validationResponse.Message;

            return response;
        }

        public async Task<DeleteMovingResponse> DeleteAsync(DeleteMovingRequest request)
        {
            var response = new DeleteMovingResponse { IsSuccess = true, Message = string.Empty };

            var moving = await _movingRepository.GetByIdAsync(request.MovingId);

            if (moving == null) return _validationMovingService.ErrorResponse(request.MovingId);

            var createMovingRequest = await CreateMovingRequestAsync(moving);

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

        private async Task<CreateMovingRequest> CreateMovingRequestAsync(Moving moving)
        {
            var movingDetails = await _movingDetailRepository.GetByMovingIdAsync(moving.Id);

            return new CreateMovingRequest
            {
                // свап (для отката остатков)
                ArrivalWarehouseId = moving.DepartureWarehouseId,
                DepartureWarehouseId = moving.ArrivalWarehouseId,
                IsDeleted = true,
                InventoryItems = movingDetails.Select(x => new CreateInventoryItemDTO
                {
                    Id = x.InventoryItemId,
                    Count = x.Count
                }).ToList()
            };
        }
    }
}
