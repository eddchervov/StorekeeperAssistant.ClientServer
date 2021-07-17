using StorekeeperAssistant.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace StorekeeperAssistant.DAL.Repositories
{
    public interface IWarehouseInventoryItemRepository : IBaseRepository<WarehouseInventoryItem>
    {
        Task<IEnumerable<WarehouseInventoryItem>> GetAsync(IEnumerable<int> warehouseIds, IEnumerable<int> inventoryItemIds, DateTime? maxDateTime = null);
    }
}
