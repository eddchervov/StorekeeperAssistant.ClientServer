using StorekeeperAssistant.DAL.Entities;
using StorekeeperAssistant.DAL.Models;
using System.Threading.Tasks;

namespace StorekeeperAssistant.DAL.Repositories
{
    public interface IMovingRepository : IBaseRepository<Moving>
    {
        Task<Moving> GetByIdAsync(int id);
        Task<Moving> GetWithMovingDetailsByIdAsync(int id);
        Task<GetMovingsResponse> GetFullAsync(int skip, int take);
    }
}
