using StorekeeperAssistant.Api.Models.Movings;
using System.Threading.Tasks;

namespace StorekeeperAssistant.BL.Services.Movings
{
    public interface IMovingService
    {
        Task<GetMovingResponse> GetAsync(GetMovingRequest request);
        Task<CreateMovingResponse> CreateAsync(CreateMovingRequest request);
        Task<DeleteMovingResponse> DeleteAsync(DeleteMovingRequest request);
    }
}
