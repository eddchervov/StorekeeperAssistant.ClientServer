using StorekeeperAssistant.Api.Models.Warehouse;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace StorekeeperAssistant.BL.Services
{
    public interface IWarehouseService
    {
        Task<GetWarehouseResponse> GetWarehousesAsync(GetWarehouseRequest request);
    }
}
