using StorekeeperAssistant.Api.Models;
using StorekeeperAssistant.Api.Models.InventoryItem;
using System.Threading.Tasks;

namespace StorekeeperAssistant.BL.Services
{
    public interface IValidationWarehouseInventoryItemService
    {
        Task<BaseResponse> ValidationWarehouseInventoryItemByWarehouseIdAsync(GetWarehouseInventoryItemByWarehouseIdRequest request);
    }
}
