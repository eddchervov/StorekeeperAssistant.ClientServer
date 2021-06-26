using System.Collections.Generic;

namespace StorekeeperAssistant.Api.Models.InventoryItems
{
    public class GetInventoryItemsResponse
    {
        public IEnumerable<InventoryItemDTO> InventoryItems { get; set; } = new List<InventoryItemDTO>();
    }
}
