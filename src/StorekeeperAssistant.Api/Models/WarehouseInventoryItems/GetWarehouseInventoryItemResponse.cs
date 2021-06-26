using System.Collections.Generic;

namespace StorekeeperAssistant.Api.Models.WarehouseInventoryItems
{
    public class GetWarehouseInventoryItemResponse : BaseResponse
    {
        public IEnumerable<WarehouseInventoryItemDTO> WarehouseInventoryItems { get; set; } = new List<WarehouseInventoryItemDTO>();
    }
}
