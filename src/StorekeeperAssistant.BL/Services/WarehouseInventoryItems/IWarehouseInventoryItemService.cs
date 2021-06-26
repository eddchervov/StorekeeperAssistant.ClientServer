using StorekeeperAssistant.Api.Models.WarehouseInventoryItems;
using System.Threading.Tasks;

namespace StorekeeperAssistant.BL.Services.WarehouseInventoryItems
{
    public interface IWarehouseInventoryItemService
    {
        Task<GetWarehouseInventoryItemResponse> GetAsync(GetWarehouseInventoryItemRequest request);
    }
}
