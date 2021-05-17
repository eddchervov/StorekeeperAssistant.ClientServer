using Microsoft.EntityFrameworkCore;
using StorekeeperAssistant.DAL.DBContext;
using StorekeeperAssistant.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StorekeeperAssistant.DAL.Repositories.Implementation
{
    internal class WarehouseInventoryItemRepository : BaseRepository<WarehouseInventoryItem>, IWarehouseInventoryItemRepository
    {
        public WarehouseInventoryItemRepository(IAppDBContext context)
            : base(context.WarehouseInventoryItems)
        {
        }

        public async Task<WarehouseInventoryItem> GetByMovingIdAsync(int movingId, int warehouseId, int nomenclatureId)
        {
            return await DbSet.FirstOrDefaultAsync(x => x.MovingId == movingId && x.WarehouseId == warehouseId && x.NomenclatureId == nomenclatureId);
        }

        public async Task<List<WarehouseInventoryItem>> GetByPeriodAsync(int warehouseId, int nomenclatureId, DateTime startDate, DateTime endDate)
        {
            return await DbSet
                .Where(x => x.DateTime > startDate
                    && x.DateTime < endDate
                    && x.IsActive == true
                    && x.WarehouseId == warehouseId
                    && x.NomenclatureId == nomenclatureId)
                    .ToListAsync();
        }
        public async Task<WarehouseInventoryItem> GetLastAsync(int warehouseId, int nomenclatureId, DateTime? maxDateTime = null)
        {
            var warehouseInventoryItems = DbSet
                .OrderByDescending(x => x.DateTime)
                .Where(x => x.WarehouseId == warehouseId && x.NomenclatureId == nomenclatureId && x.IsActive);
                
            if (maxDateTime != null)
            {
                warehouseInventoryItems = warehouseInventoryItems.Where(x => x.DateTime < maxDateTime);
            }

            return await warehouseInventoryItems.FirstOrDefaultAsync();
        }
    }
}
