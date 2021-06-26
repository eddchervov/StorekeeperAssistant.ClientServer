using StorekeeperAssistant.Api.Models.InventoryItems;

namespace StorekeeperAssistant.Api.Models.WarehouseInventoryItems
{
    public class WarehouseInventoryItemDTO
    {
        public int Id { get; set; }
        public int Count { get; set; }
        public InventoryItemDTO InventoryItem { get; set; }
    }
}
