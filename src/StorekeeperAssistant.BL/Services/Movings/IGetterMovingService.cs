using StorekeeperAssistant.Api.Models.Movings;
using System.Threading.Tasks;

namespace StorekeeperAssistant.BL.Services.Movings
{
    public interface IGetterMovingService
    {
        Task<GetMovingResponse> GetAsync(GetMovingRequest request);
    }
}
