using System;

namespace StorekeeperAssistant.Api.Models.InventoryItem
{
    public class GetWarehouseInventoryItemByWarehouseIdRequest
    {
        public int WarehouseId { get; set; }
        public DateTime? DateTime { get; set; } = null;
    }
}
