using StorekeeperAssistant.Api.Models.Nomenclature;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace StorekeeperAssistant.BL.Services
{
    public interface INomenclatureService
    {
        Task<GetNomenclaturesResponse> GetNomenclaturesAsync(GetNomenclaturesRequest request);
    }
}
