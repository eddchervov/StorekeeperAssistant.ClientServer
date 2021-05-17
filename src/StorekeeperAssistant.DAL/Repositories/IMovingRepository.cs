using StorekeeperAssistant.DAL.Entities;
using StorekeeperAssistant.DAL.Models;
using System.Threading.Tasks;

namespace StorekeeperAssistant.DAL.Repositories
{
    public interface IMovingRepository : IBaseRepository<Moving>
    {
        Task<Moving> GetByIdAsync(int id);
        Task<GetIsActiveMovingsDALResponse> GetIsActiveMovingsAsync(int skip, int take);
    }
}
