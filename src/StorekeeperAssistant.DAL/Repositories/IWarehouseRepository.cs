using StorekeeperAssistant.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace StorekeeperAssistant.DAL.Repositories
{
    public interface IWarehouseRepository
    {
        Task<Warehouse> GetByIdAsync(int id);
        Task<List<Warehouse>> GetListAsync();
    }
}
