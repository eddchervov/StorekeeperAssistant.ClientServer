using StorekeeperAssistant.Api.Models;
using StorekeeperAssistant.Api.Models.Movings;
using System.Threading.Tasks;

namespace StorekeeperAssistant.BL.Services.Movings
{
    public interface IValidationMovingService
    {
        BaseResponse Validation(GetMovingRequest request);
        DeleteMovingResponse ErrorResponse(int movingId);
        Task<BaseResponse> ValidationAsync(CreateMovingRequest request);
    }
}
