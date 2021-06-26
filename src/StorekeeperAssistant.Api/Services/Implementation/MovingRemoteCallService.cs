using Microsoft.Extensions.Configuration;
using StorekeeperAssistant.Api.Models.Movings;
using StorekeeperAssistant.Api.Utils;
using System.Threading.Tasks;

namespace StorekeeperAssistant.Api.Services.Implementation
{
    internal class MovingRemoteCallService : BaseRemoteCallService, IMovingRemoteCallService
    {
        public MovingRemoteCallService(IConfiguration configuration, ICacheProvider cacheProvider)
            : base(configuration, cacheProvider)
        { }

        protected override string _apiSchemeAndHostConfigKey { get; set; } = "StorekeeperAssistant.Api.SchemeAndHost";

        public async Task<GetMovingResponse> GetAsync(GetMovingRequest request)
            => await ExecuteGetAsync<GetMovingResponse, GetMovingRequest>("api/movings/get", request);

        public async Task<CreateMovingResponse> CreateAsync(CreateMovingRequest request)
            => await ExecutePostAsync<CreateMovingResponse, CreateMovingRequest>("api/movings/create", request);

        public async Task<DeleteMovingResponse> DeleteAsync(DeleteMovingRequest request)
            => await ExecutePutAsync<DeleteMovingResponse, DeleteMovingRequest>("api/movings/delete", request);
    }
}
