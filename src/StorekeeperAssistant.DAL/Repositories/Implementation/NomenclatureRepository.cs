using Microsoft.EntityFrameworkCore;
using StorekeeperAssistant.DAL.DBContext;
using StorekeeperAssistant.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace StorekeeperAssistant.DAL.Repositories.Implementation
{
    internal class NomenclatureRepository : BaseRepository<Nomenclature>, INomenclatureRepository
    {
        private readonly IAppDBContext _context;

        public NomenclatureRepository(IAppDBContext context)
            : base(context.Nomenclatures)
        {
            _context = context;
        }

        public async Task<List<Nomenclature>> GetListAsync()
        {
            return await DbSet.ToListAsync();
        }

        public async Task<Nomenclature> GetByIdAsync(int id)
        {
            return await DbSet.FirstOrDefaultAsync(x => x.Id == id);
        }
    }
}
