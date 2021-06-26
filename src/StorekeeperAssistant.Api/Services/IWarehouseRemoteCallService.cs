using StorekeeperAssistant.Api.Models.Warehouses;
using System.Threading.Tasks;

namespace StorekeeperAssistant.Api.Services
{
    public interface IWarehouseRemoteCallService
    {
        Task<GetWarehouseResponse> GetWithoutCacheAsync(GetWarehouseRequest request);
        Task<GetWarehouseResponse> GetAsync(GetWarehouseRequest request);
    }
}
