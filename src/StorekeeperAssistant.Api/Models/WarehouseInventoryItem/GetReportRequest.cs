using System;
using System.Collections.Generic;
using System.Text;

namespace StorekeeperAssistant.Api.Models.WarehouseInventoryItem
{
    public class GetReportRequest
    {
        public int WarehouseId { get; set; }
        public DateTime DateTime { get; set; }
    }
}
