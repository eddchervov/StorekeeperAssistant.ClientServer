using Microsoft.Extensions.Configuration;
using StorekeeperAssistant.Api.Models.WarehouseInventoryItems;
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

        public async Task<GetWarehouseInventoryItemResponse> GetAsync(GetWarehouseInventoryItemRequest request)
            => await ExecuteGetAsync<GetWarehouseInventoryItemResponse, GetWarehouseInventoryItemRequest>("api/warehouse-inventory-items/get", request);
    }
}
