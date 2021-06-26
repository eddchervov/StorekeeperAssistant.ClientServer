using System;

namespace StorekeeperAssistant.Api.Models.WarehouseInventoryItems
{
    public class GetWarehouseInventoryItemRequest
    {
        public int WarehouseId { get; set; }
        public DateTime? DateTime { get; set; }
    }
}
