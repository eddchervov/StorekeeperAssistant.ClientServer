using Microsoft.Extensions.Configuration;
using StorekeeperAssistant.Api.Models.InventoryItem;
using StorekeeperAssistant.Api.Utils;
using System.Threading.Tasks;

namespace StorekeeperAssistant.Api.Services.Implementation
{
    internal class WarehouseInventoryItemRemoteCallService : BaseRemoteCallService, IWarehouseInventoryItemRemoteCallService
    {
        public WarehouseInventoryItemRemoteCallService(IConfiguration configuration, ICacheProvider cacheProvider)
            : base(configuration, cacheProvider)
        { }

        protected override string _apiSchemeAndHostConfigKey { get; set; } = "StorekeeperAssistant.Api.SchemeAndHost";

        public async Task<GetWarehouseInventoryItemByWarehouseIdResponse> GetWarehouseInventoryItemByWarehouseIdAsync(GetWarehouseInventoryItemByWarehouseIdRequest request)
            => await ExecuteGetAsync<GetWarehouseInventoryItemByWarehouseIdResponse, GetWarehouseInventoryItemByWarehouseIdRequest>("api/warehouse-inventory-item/get-by-warehouse-id", request);
    }
}
