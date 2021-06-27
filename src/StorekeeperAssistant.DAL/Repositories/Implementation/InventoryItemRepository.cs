using Microsoft.EntityFrameworkCore;
using StorekeeperAssistant.DAL.DBContext;
using StorekeeperAssistant.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StorekeeperAssistant.DAL.Repositories.Implementation
{
    internal class InventoryItemRepository : BaseRepository<InventoryItem>, IInventoryItemRepository
    {
        public InventoryItemRepository(IAppDBContext context)
            : base(context.InventoryItems)
        {
        }

        public async Task<IEnumerable<InventoryItem>> GetAsync()
        {
            return await DbSet.Where(x => x.IsDeleted == false).ToListAsync();
        }

        public async Task<InventoryItem> GetByIdAsync(int id)
        {
            return await DbSet.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<IEnumerable<InventoryItem>> GetByIdsAsync(IEnumerable<int> ids)
        {
            return await DbSet.Where(x => x.IsDeleted == false && ids.Contains(x.Id)).ToListAsync();
        }
    }
}
