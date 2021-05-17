using StorekeeperAssistant.Api.Models.Warehouse;
using System.Threading.Tasks;

namespace StorekeeperAssistant.Api.Services
{
    public interface IWarehouseRemoteCallService
    {
        Task<GetWarehouseResponse> GetWarehousesWithoutCacheAsync(GetWarehouseRequest request);
        Task<GetWarehouseResponse> GetWarehousesAsync(GetWarehouseRequest request);
    }
}
