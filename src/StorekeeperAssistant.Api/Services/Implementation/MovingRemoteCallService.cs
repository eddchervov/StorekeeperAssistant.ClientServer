using Microsoft.Extensions.Configuration;
using StorekeeperAssistant.Api.Models.Moving;
using StorekeeperAssistant.Api.Utils;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace StorekeeperAssistant.Api.Services.Implementation
{
    internal class MovingRemoteCallService : BaseRemoteCallService, IMovingRemoteCallService
    {
        public MovingRemoteCallService(IConfiguration configuration, ICacheProvider cacheProvider)
            : base(configuration, cacheProvider)
        { }

        protected override string _apiSchemeAndHostConfigKey { get; set; } = "StorekeeperAssistant.Api.SchemeAndHost";

        public async Task<GetMovingResponse> GetMovingsAsync(GetMovingRequest request)
            => await ExecuteGetAsync<GetMovingResponse, GetMovingRequest>("api/movings/get", request);

        public async Task<CreateMovingResponse> CreateMovingAsync(CreateMovingRequest request)
            => await ExecutePostAsync<CreateMovingResponse, CreateMovingRequest>("api/movings/create", request);

        public async Task<DeleteMovingResponse> DeleteMovingAsync(DeleteMovingRequest request)
            => await ExecutePutAsync<DeleteMovingResponse, DeleteMovingRequest>("api/movings/delete", request);
    }
}
