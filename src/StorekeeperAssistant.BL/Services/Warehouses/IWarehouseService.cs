using StorekeeperAssistant.Api.Models.Warehouses;
using System.Threading.Tasks;

namespace StorekeeperAssistant.BL.Services.Warehouses
{
    public interface IWarehouseService
    {
        Task<GetWarehouseResponse> GetAsync(GetWarehouseRequest request);
    }
}
