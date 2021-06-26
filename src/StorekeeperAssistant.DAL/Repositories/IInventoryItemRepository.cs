using StorekeeperAssistant.DAL.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace StorekeeperAssistant.DAL.Repositories
{
    public interface IInventoryItemRepository
    {
        Task<IEnumerable<InventoryItem>> GetAsync();
        Task<InventoryItem> GetByIdAsync(int id);
        Task<IEnumerable<InventoryItem>> GetByIdsAsync(IEnumerable<int> ids);
    }
}
