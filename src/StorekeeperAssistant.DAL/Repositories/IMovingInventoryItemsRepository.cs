using StorekeeperAssistant.DAL.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace StorekeeperAssistant.DAL.Repositories
{
    public interface IMovingInventoryItemsRepository
    {
        Task<List<InventoryItemsDALModel>> GetInventoryItemsByMovingIdAsync(int movingId);
    }
}
