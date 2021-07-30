using StorekeeperAssistant.DAL.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace StorekeeperAssistant.DAL.Repositories
{
    public interface IWarehouseRepository : IBaseRepository<Warehouse>
    {
        Task<Warehouse> GetByIdAsync(int id);
        Task<IEnumerable<Warehouse>> GetByIdsAsync(IEnumerable<int> ids);
        Task<IEnumerable<Warehouse>> GetAsync();
    }
}
