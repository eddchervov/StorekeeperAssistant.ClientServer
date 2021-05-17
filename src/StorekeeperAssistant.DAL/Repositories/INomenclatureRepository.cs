using StorekeeperAssistant.DAL.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace StorekeeperAssistant.DAL.Repositories
{
    public interface INomenclatureRepository
    {
        Task<List<Nomenclature>> GetListAsync();
        Task<Nomenclature> GetByIdAsync(int id);
        Task<List<Nomenclature>> GetByIdsAsync(IEnumerable<int> ids);
    }
}
