using StorekeeperAssistant.Api.Models.WarehouseInventoryItems;
using System.Threading.Tasks;

namespace StorekeeperAssistant.Api.Services
{
    public interface IWarehouseInventoryItemRemoteCallService
    {
        Task<GetWarehouseInventoryItemResponse> GetAsync(GetWarehouseInventoryItemRequest request);
    }
}
