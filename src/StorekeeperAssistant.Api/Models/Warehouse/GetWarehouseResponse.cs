using System;
using System.Collections.Generic;
using System.Text;

namespace StorekeeperAssistant.Api.Models.Warehouse
{
    public class GetWarehouseResponse
    {
        public List<WarehouseModel> WarehouseModels { get; set; } = new List<WarehouseModel>();
    }
}
