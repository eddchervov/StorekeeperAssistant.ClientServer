using StorekeeperAssistant.DAL.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace StorekeeperAssistant.DAL.Repositories
{
    public interface IMovingDetailRepository : IBaseRepository<MovingDetail>
    {
        Task<List<MovingDetail>> GetByMovingIdAsync(int movingId);
        Task<List<MovingDetail>> GetByMovingIdsAsync(IEnumerable<int> movingIds);
    }
}
