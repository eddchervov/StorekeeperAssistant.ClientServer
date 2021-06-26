using StorekeeperAssistant.Api.Models.Movings;
using System.Threading.Tasks;

namespace StorekeeperAssistant.Api.Services
{
    public interface IMovingRemoteCallService
    {
        Task<GetMovingResponse> GetAsync(GetMovingRequest request);
        Task<CreateMovingResponse> CreateAsync(CreateMovingRequest request);
        Task<DeleteMovingResponse> DeleteAsync(DeleteMovingRequest request);
    }
}
