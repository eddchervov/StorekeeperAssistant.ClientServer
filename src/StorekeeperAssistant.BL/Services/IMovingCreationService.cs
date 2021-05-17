﻿using StorekeeperAssistant.Api.Models.Moving;
using System.Threading.Tasks;

namespace StorekeeperAssistant.BL.Services
{
    public interface IMovingCreationService
    {
        Task<CreateMovingResponse> CreateAsync(CreateMovingRequest request);
    }
}
