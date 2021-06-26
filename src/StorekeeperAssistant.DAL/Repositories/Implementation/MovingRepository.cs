using Microsoft.EntityFrameworkCore;
using StorekeeperAssistant.DAL.DBContext;
using StorekeeperAssistant.DAL.Entities;
using StorekeeperAssistant.DAL.Models;
using System.Linq;
using System.Threading.Tasks;

namespace StorekeeperAssistant.DAL.Repositories.Implementation
{
    internal class MovingRepository : BaseRepository<Moving>, IMovingRepository
    {
        public MovingRepository(IAppDBContext context)
            : base(context.Movings)
        {
        }

        public async Task<Moving> GetByIdAsync(int id)
        {
            return await DbSet.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<GetMovingsResponse> GetFullAsync(int skip, int take)
        {
            var q = DbSet
                .Include(x => x.ArrivalWarehouse)
                .Include(x => x.DepartureWarehouse)
                .Where(m => m.IsDeleted == false);
            q = q.OrderByDescending(x => x.DateTime);

            var totalCount = await q.CountAsync();

            q = q.Skip(skip).Take(take);

            return new GetMovingsResponse { TotalCount = totalCount, Movings = await q.ToListAsync() };
        }
    }
}
