using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using StorekeeperAssistant.Api.Models.Warehouses;
using StorekeeperAssistant.Api.Utils;
using System.Threading.Tasks;

namespace StorekeeperAssistant.Api.Services.Implementation
{
    internal class WarehouseRemoteCallService : BaseRemoteCallService, IWarehouseRemoteCallService
    {
        public WarehouseRemoteCallService(IConfiguration configuration, ICacheProvider cacheProvider)
            : base(configuration, cacheProvider)
        { }

        protected override string _apiSchemeAndHostConfigKey { get; set; } = "StorekeeperAssistant.Api.SchemeAndHost";

        public async Task<GetWarehouseResponse> GetWithoutCacheAsync(GetWarehouseRequest request)
            => await ExecuteGetAsync<GetWarehouseResponse, GetWarehouseRequest>("api/warehouses/get", request);

        public async Task<GetWarehouseResponse> GetAsync(GetWarehouseRequest request)
        {
            var cacheKey = $"{nameof(WarehouseRemoteCallService)}.{nameof(GetWarehouseRequest)}({JsonConvert.SerializeObject(request)})";

            return await _cacheProvider.Get(cacheKey, _defaultCacheTime, () => GetWithoutCacheAsync(request));
        }
    }
}
