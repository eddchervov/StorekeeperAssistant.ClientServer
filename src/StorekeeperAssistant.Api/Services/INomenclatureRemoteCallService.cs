using StorekeeperAssistant.Api.Models.Nomenclature;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace StorekeeperAssistant.Api.Services
{
    public interface INomenclatureRemoteCallService
    {
        Task<GetNomenclaturesResponse> GetNomenclaturesWithoutCacheAsync(GetNomenclaturesRequest request);
        Task<GetNomenclaturesResponse> GetNomenclaturesAsync(GetNomenclaturesRequest request);
    }
}
