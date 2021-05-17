using StorekeeperAssistant.Api.Models.InventoryItem;
using System.Threading.Tasks;

namespace StorekeeperAssistant.BL.Services
{
    public interface IWarehouseInventoryItemService
    {
        Task<GetWarehouseInventoryItemByWarehouseIdResponse> GetWarehouseInventoryItemByWarehouseIdAsync(GetWarehouseInventoryItemByWarehouseIdRequest request);
    }
}
