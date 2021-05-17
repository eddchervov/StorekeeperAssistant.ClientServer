using Microsoft.EntityFrameworkCore;
using StorekeeperAssistant.DAL.DBContext;
using StorekeeperAssistant.DAL.Entities;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StorekeeperAssistant.DAL.Repositories.Implementation
{
    internal class MovingDetailRepository : BaseRepository<MovingDetail>, IMovingDetailRepository
    {
        public MovingDetailRepository(IAppDBContext context)
            : base(context.MovingDetails)
        {
        }

        public async Task<List<MovingDetail>> GetByMovingIdAsync(int movingId)
        {
            return await DbSet.Where(x=>x.MovingId == movingId).ToListAsync();
        }

        public async Task<List<MovingDetail>> GetByMovingIdsAsync(IEnumerable<int> movingIds)
        {
            return await DbSet.Where(x => movingIds.Contains(x.MovingId)).ToListAsync();
        }
    }
}
