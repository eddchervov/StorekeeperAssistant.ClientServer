using StorekeeperAssistant.Api.Models;
using StorekeeperAssistant.Api.Models.Moving;
using System.Threading.Tasks;

namespace StorekeeperAssistant.BL.Services
{
    public interface IValidationMovingService
    {
        Task<BaseResponse> ValidationCreateMovingAsync(CreateMovingRequest request);
    }
}
