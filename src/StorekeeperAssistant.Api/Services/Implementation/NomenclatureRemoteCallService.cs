using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using StorekeeperAssistant.Api.Models.Nomenclature;
using StorekeeperAssistant.Api.Utils;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace StorekeeperAssistant.Api.Services.Implementation
{
    internal class NomenclatureRemoteCallService : BaseRemoteCallService, INomenclatureRemoteCallService
    {
        public NomenclatureRemoteCallService(IConfiguration configuration, ICacheProvider cacheProvider)
            : base(configuration, cacheProvider)
        { }

        protected override string _apiSchemeAndHostConfigKey { get; set; } = "StorekeeperAssistant.Api.SchemeAndHost";

        public async Task<GetNomenclaturesResponse> GetNomenclaturesWithoutCacheAsync(GetNomenclaturesRequest request)
            => await ExecuteGetAsync<GetNomenclaturesResponse, GetNomenclaturesRequest>("api/nomenclature/get", request);

        public async Task<GetNomenclaturesResponse> GetNomenclaturesAsync(GetNomenclaturesRequest request)
        {
            var cacheKey = $"{nameof(NomenclatureRemoteCallService)}.{nameof(GetNomenclaturesRequest)}({JsonConvert.SerializeObject(request)})";

            return await _cacheProvider.Get(cacheKey, _defaultCacheTime, () => GetNomenclaturesWithoutCacheAsync(request));
        }
    }
}
