using System.Collections.Generic;

namespace StorekeeperAssistant.Api.Models.Warehouses
{
    public class GetWarehouseResponse
    {
        public IEnumerable<WarehouseDTO> Warehouses { get; set; } = new List<WarehouseDTO>();
    }
}
