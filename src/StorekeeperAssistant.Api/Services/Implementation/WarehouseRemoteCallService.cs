using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using StorekeeperAssistant.Api.Models.Warehouse;
using StorekeeperAssistant.Api.Utils;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace StorekeeperAssistant.Api.Services.Implementation
{
    internal class WarehouseRemoteCallService : BaseRemoteCallService, IWarehouseRemoteCallService
    {
        public WarehouseRemoteCallService(IConfiguration configuration, ICacheProvider cacheProvider)
            : base(configuration, cacheProvider)
        { }

        protected override string _apiSchemeAndHostConfigKey { get; set; } = "StorekeeperAssistant.Api.SchemeAndHost";

        public async Task<GetWarehouseResponse> GetWarehousesWithoutCacheAsync(GetWarehouseRequest request)
            => await ExecuteGetAsync<GetWarehouseResponse, GetWarehouseRequest>("api/warehouse/get", request);

        public async Task<GetWarehouseResponse> GetWarehousesAsync(GetWarehouseRequest request)
        {
            var cacheKey = $"{nameof(WarehouseRemoteCallService)}.{nameof(GetWarehouseRequest)}({JsonConvert.SerializeObject(request)})";

            return await _cacheProvider.Get(cacheKey, _defaultCacheTime, () => GetWarehousesWithoutCacheAsync(request));
        }
    }
}
