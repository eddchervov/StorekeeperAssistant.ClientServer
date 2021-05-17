using System;

namespace StorekeeperAssistant.Api.Models.WarehouseInventoryItem
{
    public class GetReportRequest
    {
        public int WarehouseId { get; set; }
        public DateTime DateTime { get; set; }
    }
}
