using StorekeeperAssistant.Api.Models.Moving;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace StorekeeperAssistant.Api.Services
{
    public interface IMovingRemoteCallService
    {
        Task<GetMovingResponse> GetMovingsAsync(GetMovingRequest request);
        Task<CreateMovingResponse> CreateMovingAsync(CreateMovingRequest request);
        Task<DeleteMovingResponse> DeleteMovingAsync(DeleteMovingRequest request);
    }
}
