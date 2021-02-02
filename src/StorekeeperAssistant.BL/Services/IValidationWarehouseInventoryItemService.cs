using StorekeeperAssistant.Api.Models;
using StorekeeperAssistant.Api.Models.InventoryItem;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace StorekeeperAssistant.BL.Services
{
    public interface IValidationWarehouseInventoryItemService
    {
        Task<BaseResponse> ValidationWarehouseInventoryItemByWarehouseIdAsync(GetWarehouseInventoryItemByWarehouseIdRequest request);
    }
}
