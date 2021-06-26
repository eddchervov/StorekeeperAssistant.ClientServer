using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using StorekeeperAssistant.Api.Models.InventoryItems;
using StorekeeperAssistant.Api.Utils;
using System.Threading.Tasks;

namespace StorekeeperAssistant.Api.Services.Implementation
{
    internal class InventoryItemRemoteCallService : BaseRemoteCallService, IInventoryItemRemoteCallService
    {
        public InventoryItemRemoteCallService(IConfiguration configuration, ICacheProvider cacheProvider)
            : base(configuration, cacheProvider)
        { }

        protected override string _apiSchemeAndHostConfigKey { get; set; } = "StorekeeperAssistant.Api.SchemeAndHost";

        public async Task<GetInventoryItemsResponse> GetWithoutCacheAsync(GetInventoryItemsRequest request)
            => await ExecuteGetAsync<GetInventoryItemsResponse, GetInventoryItemsRequest>("api/inventory-items/get", request);

        public async Task<GetInventoryItemsResponse> GetAsync(GetInventoryItemsRequest request)
        {
            var cacheKey = $"{nameof(InventoryItemRemoteCallService)}.{nameof(GetInventoryItemsRequest)}({JsonConvert.SerializeObject(request)})";

            return await _cacheProvider.Get(cacheKey, _defaultCacheTime, () => GetWithoutCacheAsync(request));
        }
    }
}
