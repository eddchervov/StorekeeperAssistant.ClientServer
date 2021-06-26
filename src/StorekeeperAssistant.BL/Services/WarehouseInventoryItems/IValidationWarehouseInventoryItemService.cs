using StorekeeperAssistant.Api.Models;
using StorekeeperAssistant.Api.Models.WarehouseInventoryItems;
using System.Threading.Tasks;

namespace StorekeeperAssistant.BL.Services.WarehouseInventoryItems
{
    public interface IValidationWarehouseInventoryItemService
    {
        Task<BaseResponse> ValidationAsync(GetWarehouseInventoryItemRequest request);
    }
}
