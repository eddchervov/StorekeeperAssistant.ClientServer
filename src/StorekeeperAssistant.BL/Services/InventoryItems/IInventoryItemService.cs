using StorekeeperAssistant.Api.Models.InventoryItems;
using System.Threading.Tasks;

namespace StorekeeperAssistant.BL.Services.InventoryItems
{
    public interface IInventoryItemService
    {
        Task<GetInventoryItemsResponse> GetAsync(GetInventoryItemsRequest request);
    }
}
