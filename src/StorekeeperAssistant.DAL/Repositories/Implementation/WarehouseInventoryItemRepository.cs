﻿using Microsoft.EntityFrameworkCore;
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

        public async Task<IEnumerable<WarehouseInventoryItem>> GetAsync(IEnumerable<int> warehouseIds, IEnumerable<int> inventoryItemIds, DateTime? maxDateTime = null)
        {
            var warehouseInventoryItems = DbSet
                .OrderByDescending(x => x.DateTime)
                .Where(x => warehouseIds.Contains(x.WarehouseId) && inventoryItemIds.Contains(x.InventoryItemId));

            if (maxDateTime != null)
            {
                warehouseInventoryItems = warehouseInventoryItems.Where(x => x.DateTime <= maxDateTime);
            }

            return await warehouseInventoryItems.ToListAsync();
        }
    }
}
