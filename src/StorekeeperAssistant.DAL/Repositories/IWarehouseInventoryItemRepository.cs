using StorekeeperAssistant.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace StorekeeperAssistant.DAL.Repositories
{
    public interface IWarehouseInventoryItemRepository : IBaseRepository<WarehouseInventoryItem>
    {
        Task<WarehouseInventoryItem> GetByMovingIdAsync(int movingId, int warehouseId, int nomenclatureId);
        Task<List<WarehouseInventoryItem>> GetByPeriodAsync(int warehouseId, int nomenclatureId, DateTime startDate, DateTime endDate);
        Task<WarehouseInventoryItem> GetLastAsync(int warehouseId, int nomenclatureId, DateTime? maxDateTime = null);
    }
}
