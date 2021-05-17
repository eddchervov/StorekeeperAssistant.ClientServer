using StorekeeperAssistant.Api.Models.Warehouse;
using System.Threading.Tasks;

namespace StorekeeperAssistant.BL.Services
{
    public interface IWarehouseService
    {
        Task<GetWarehouseResponse> GetWarehousesAsync(GetWarehouseRequest request);
    }
}
