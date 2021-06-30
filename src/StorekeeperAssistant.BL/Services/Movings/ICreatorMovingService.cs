using StorekeeperAssistant.Api.Models.Movings;
using System.Threading.Tasks;

namespace StorekeeperAssistant.BL.Services.Movings
{
    public interface ICreatorMovingService
    {
        Task CreateAsync(CreateMovingRequest request);
    }
}
