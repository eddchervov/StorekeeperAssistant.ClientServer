using Microsoft.Extensions.Configuration;
using StorekeeperAssistant.Api.Models.Moving;
using StorekeeperAssistant.Api.Utils;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace StorekeeperAssistant.Api.Services.Implementation
{
    class MovingRemoteCallService : BaseRemoteCallService, IMovingRemoteCallService
    {
        public MovingRemoteCallService(IConfiguration configuration, ICacheProvider cacheProvider)
            : base(configuration, cacheProvider)
        { }

        protected override string _apiSchemeAndHostConfigKey { get; set; } = "StorekeeperAssistant.Api.SchemeAndHost";

        public async Task<GetMovingResponse> GetMovingsAsync(GetMovingRequest request)
            => await ExecuteGetAsync<GetMovingResponse, GetMovingRequest>("/api/moving/get", request);
    }
}
