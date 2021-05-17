using StorekeeperAssistant.Api.Models.InventoryItem;
using System.Threading.Tasks;

namespace StorekeeperAssistant.Api.Services
{
    public interface IWarehouseInventoryItemRemoteCallService
    {
        Task<GetWarehouseInventoryItemByWarehouseIdResponse> GetWarehouseInventoryItemByWarehouseIdAsync(GetWarehouseInventoryItemByWarehouseIdRequest request);
    }
}
