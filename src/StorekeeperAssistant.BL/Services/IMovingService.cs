﻿using StorekeeperAssistant.Api.Models.Moving;
using System.Threading.Tasks;

namespace StorekeeperAssistant.BL.Services
{
    public interface IMovingService
    {
        Task<GetMovingResponse> GetMovingsAsync(GetMovingRequest request);
        Task<CreateMovingResponse> CreateMovingAsync(CreateMovingRequest request);
        Task<DeleteMovingResponse> DeleteMovingAsync(DeleteMovingRequest request);
    }
}
