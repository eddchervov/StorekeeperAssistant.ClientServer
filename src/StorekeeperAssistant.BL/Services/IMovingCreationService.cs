using StorekeeperAssistant.Api.Models.Moving;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace StorekeeperAssistant.BL.Services
{
    public interface IMovingCreationService
    {
        Task<CreateMovingResponse> CreateAsync(CreateMovingRequest request);
    }
}
