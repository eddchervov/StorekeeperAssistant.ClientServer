using StorekeeperAssistant.DAL.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace StorekeeperAssistant.DAL.Repositories
{
    public interface IMovingDetailRepository : IBaseRepository<MovingDetail>
    {
        Task<IEnumerable<MovingDetail>> GetByMovingIdAsync(int movingId);
        Task<IEnumerable<MovingDetail>> GetByMovingIdsAsync(IEnumerable<int> movingIds);
    }
}
