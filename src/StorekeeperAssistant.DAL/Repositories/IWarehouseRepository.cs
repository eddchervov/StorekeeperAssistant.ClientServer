using StorekeeperAssistant.DAL.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace StorekeeperAssistant.DAL.Repositories
{
    public interface IWarehouseRepository
    {
        Task<Warehouse> GetByIdAsync(int id);
        Task<List<Warehouse>> GetByIdsAsync(IEnumerable<int> ids);
        Task<List<Warehouse>> GetListAsync();
    }
}
