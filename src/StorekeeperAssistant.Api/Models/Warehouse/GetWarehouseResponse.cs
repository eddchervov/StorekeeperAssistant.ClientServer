using System.Collections.Generic;

namespace StorekeeperAssistant.Api.Models.Warehouse
{
    public class GetWarehouseResponse
    {
        public List<WarehouseModel> WarehouseModels { get; set; } = new List<WarehouseModel>();
    }
}
