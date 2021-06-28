using StorekeeperAssistant.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace StorekeeperAssistant.DAL.Repositories
{
    public interface IWarehouseInventoryItemRepository : IBaseRepository<WarehouseInventoryItem>
    {
        Task<IEnumerable<WarehouseInventoryItem>> GetLastesAsync(IEnumerable<int> warehouseIds, IEnumerable<int> inventoryItemIds, DateTime? maxDateTime = null);
        Task<WarehouseInventoryItem> GetLastAsync(int warehouseId, int inventoryItemId, DateTime? maxDateTime = null);
    }
}
