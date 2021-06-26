using StorekeeperAssistant.Api.Models.InventoryItems;
using System.Threading.Tasks;

namespace StorekeeperAssistant.Api.Services
{
    public interface IInventoryItemRemoteCallService
    {
        Task<GetInventoryItemsResponse> GetWithoutCacheAsync(GetInventoryItemsRequest request);
        Task<GetInventoryItemsResponse> GetAsync(GetInventoryItemsRequest request);
    }
}
