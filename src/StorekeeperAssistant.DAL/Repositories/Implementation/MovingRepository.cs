using StorekeeperAssistant.DAL.DBContext;
using StorekeeperAssistant.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using StorekeeperAssistant.DAL.Models;

namespace StorekeeperAssistant.DAL.Repositories.Implementation
{
    internal class MovingRepository : BaseRepository<Moving>, IMovingRepository
    {
        private readonly IAppDBContext _context;

        public MovingRepository(IAppDBContext context)
            : base(context.Movings)
        {
            _context = context;
        }

        public async Task<GetIsActiveMovingsDALResponse> GetIsActiveMovingsAsync(int skip, int take)
        {
            var q = from m in _context.Movings
                    where m.IsActive == true
                    select m;

            var totalCount = await q.CountAsync();

            q = q.Skip(skip).Take(take);

            return new GetIsActiveMovingsDALResponse { TotalCount = totalCount,  Movings = await q.ToListAsync() };
        }
    }
}
