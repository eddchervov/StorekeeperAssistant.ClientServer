using Microsoft.EntityFrameworkCore;
using StorekeeperAssistant.DAL.DBContext;
using StorekeeperAssistant.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StorekeeperAssistant.DAL.Repositories.Implementation
{
    internal class NomenclatureRepository : BaseRepository<Nomenclature>, INomenclatureRepository
    {
        public NomenclatureRepository(IAppDBContext context)
            : base(context.Nomenclatures)
        {
        }

        public async Task<List<Nomenclature>> GetListAsync()
        {
            return await DbSet.ToListAsync();
        }

        public async Task<Nomenclature> GetByIdAsync(int id)
        {
            return await DbSet.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<List<Nomenclature>> GetByIdsAsync(IEnumerable<int> ids)
        {
            return await DbSet.Where(x => ids.Contains(x.Id)).ToListAsync();
        }
    }
}
