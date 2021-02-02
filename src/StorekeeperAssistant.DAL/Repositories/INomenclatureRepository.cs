using StorekeeperAssistant.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace StorekeeperAssistant.DAL.Repositories
{
    public interface INomenclatureRepository
    {
        Task<List<Nomenclature>> GetListAsync();
        Task<Nomenclature> GetByIdAsync(int id);
    }
}
