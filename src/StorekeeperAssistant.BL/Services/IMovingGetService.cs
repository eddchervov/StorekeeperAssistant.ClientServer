using StorekeeperAssistant.Api.Models.Moving;
using System.Threading.Tasks;

namespace StorekeeperAssistant.BL.Services
{
    public interface IMovingGetService
    {
        Task<GetMovingResponse> GetMovingsAsync(GetMovingRequest request);
    }
}
