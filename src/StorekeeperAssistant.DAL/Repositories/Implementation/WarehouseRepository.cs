using Microsoft.EntityFrameworkCore;
using StorekeeperAssistant.DAL.DBContext;
using StorekeeperAssistant.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StorekeeperAssistant.DAL.Repositories.Implementation
{
    internal class WarehouseRepository : BaseRepository<Warehouse>, IWarehouseRepository
    {
        public WarehouseRepository(IAppDBContext context)
            : base(context.Warehouses)
        {
        }

        public async Task<Warehouse> GetByIdAsync(int id)
        {
            return await DbSet.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<IEnumerable<Warehouse>> GetByIdsAsync(IEnumerable<int> ids)
        {
            return await DbSet.Where(x => x.IsDeleted == false && ids.Contains(x.Id)).ToListAsync();
        }

        public async Task<IEnumerable<Warehouse>> GetAsync()
        {
            return await DbSet.Where(x => x.IsDeleted == false).ToListAsync();
        }
    }
}
