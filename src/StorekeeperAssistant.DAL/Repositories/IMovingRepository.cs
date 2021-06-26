using StorekeeperAssistant.DAL.Entities;
using StorekeeperAssistant.DAL.Models;
using System.Threading.Tasks;

namespace StorekeeperAssistant.DAL.Repositories
{
    public interface IMovingRepository : IBaseRepository<Moving>
    {
        Task<Moving> GetByIdAsync(int id);
        Task<MovingsDTOResponse> GetAsync(int skip, int take);
    }
}
