using StorekeeperAssistant.Api.Models.InventoryItem;
using StorekeeperAssistant.Api.Models.Moving;
using StorekeeperAssistant.DAL.Repositories;
using System.Linq;
using System.Threading.Tasks;

namespace StorekeeperAssistant.BL.Services.Implementation
{
    internal class MovingService : IMovingService
    {
        private readonly IValidationMovingService _validationMovingService;
        private readonly IMovingCreationService _movingCreationService;
        private readonly IMovingGetService _movingGetService;
        private readonly IMovingDetailRepository _movingDetailRepository;
        private readonly IMovingRepository _movingRepository;

        public MovingService(IValidationMovingService validationMovingService,
            IMovingCreationService movingCreationService,
            IMovingGetService movingGetService,
            IMovingDetailRepository movingDetailRepository,
            IMovingRepository movingRepository)
        {
            _validationMovingService = validationMovingService;
            _movingCreationService = movingCreationService;
            _movingGetService = movingGetService;
            _movingDetailRepository = movingDetailRepository;
            _movingRepository = movingRepository;
        }

        public async Task<GetMovingResponse> GetMovingsAsync(GetMovingRequest request)
        {
            var response = await _movingGetService.GetMovingsAsync(request);

            return response;
        }

        public async Task<CreateMovingResponse> CreateMovingAsync(CreateMovingRequest request)
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

        public async Task<DeleteMovingResponse> DeleteMovingAsync(DeleteMovingRequest request)
        {
            var response = new DeleteMovingResponse { IsSuccess = true, Message = string.Empty };

            var moving = await _movingRepository.GetByIdAsync(request.MovingId);
            var movingDetails = await _movingDetailRepository.GetByMovingIdAsync(moving.Id);

            var requestCreateMoving = new CreateMovingRequest
            {
                // меняем местами (для отката остатков)
                ArrivalWarehouseId = moving.DepartureWarehouseId,
                DepartureWarehouseId = moving.ArrivalWarehouseId,
                IsActive = false,
                CreateInventoryItemModels = movingDetails.Select(x => new CreateInventoryItemModel
                {
                    Id = x.NomenclatureId,
                    Count = x.Count
                }).ToList()
            };

            var responseCreateMoving = await CreateMovingAsync(requestCreateMoving);
            if (responseCreateMoving.IsSuccess == false)
            {
                response.IsSuccess = false;
                response.Message = responseCreateMoving.Message;
                return response;
            }

            moving.IsActive = false;
            await _movingRepository.UpdateAsync(moving);

            return response;
        }
    }
}
