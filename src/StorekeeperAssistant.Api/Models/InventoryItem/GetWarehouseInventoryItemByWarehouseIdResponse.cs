using System;
using System.Collections.Generic;
using System.Text;

namespace StorekeeperAssistant.Api.Models.InventoryItem
{
    public class GetWarehouseInventoryItemByWarehouseIdResponse : BaseResponse
    {
        public List<InventoryItemModel> InventoryItemModels { get; set; } = new List<InventoryItemModel>();
    }
}
