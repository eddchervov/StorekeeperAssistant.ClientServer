using Microsoft.EntityFrameworkCore;
using StorekeeperAssistant.DAL.DBContext;
using StorekeeperAssistant.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace StorekeeperAssistant.DAL.Repositories.Implementation
{
    internal class WarehouseRepository : BaseRepository<Warehouse>, IWarehouseRepository
    {
        private readonly IAppDBContext _context;

        public WarehouseRepository(IAppDBContext context)
            : base(context.Warehouses)
        {
            _context = context;
        }

        public async Task<Warehouse> GetByIdAsync(int id)
        {
            return await DbSet.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<List<Warehouse>> GetListAsync()
        {
            return await DbSet.ToListAsync();
        }
    }
}
