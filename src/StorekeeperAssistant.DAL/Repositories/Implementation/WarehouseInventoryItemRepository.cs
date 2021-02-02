using StorekeeperAssistant.DAL.DBContext;
using StorekeeperAssistant.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace StorekeeperAssistant.DAL.Repositories.Implementation
{
    internal class WarehouseInventoryItemRepository : BaseRepository<WarehouseInventoryItem>, IWarehouseInventoryItemRepository
    {
        private readonly IAppDBContext _context;

        public WarehouseInventoryItemRepository(IAppDBContext context)
            : base(context.WarehouseInventoryItems)
        {
            _context = context;
        }

        public async Task<WarehouseInventoryItem> GetByMovingIdAsync(int movingId, int warehouseId, int nomenclatureId)
        {
            return await DbSet.FirstOrDefaultAsync(x => x.MovingId == movingId && x.WarehouseId == warehouseId && x.NomenclatureId == nomenclatureId);
        }

        public async Task<List<WarehouseInventoryItem>> GetByPeriodAsync(int warehouseId, int nomenclatureId, DateTime startDate, DateTime endDate)
        {
            return await _context.WarehouseInventoryItems
                .Where(x => x.DateTime > startDate
                    && x.DateTime < endDate
                    && x.IsActive == true
                    && x.WarehouseId == warehouseId
                    && x.NomenclatureId == nomenclatureId)
                    .ToListAsync();
        }
        public async Task<WarehouseInventoryItem> GetLastByWarehouseIdAndNomenclatureIdAsync(int warehouseId, int nomenclatureId, DateTime? dateTime = null)
        {
            var wii = DbSet.OrderByDescending(x => x.DateTime)
                .Where(x => x.WarehouseId == warehouseId && x.NomenclatureId == nomenclatureId && x.IsActive == true);

            if (dateTime != null)
            {
                wii = wii.Where(x => x.DateTime < dateTime);
            }

            return await wii.FirstOrDefaultAsync();
        }
    }
}
