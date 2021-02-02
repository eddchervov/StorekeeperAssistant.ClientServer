using Microsoft.EntityFrameworkCore;
using StorekeeperAssistant.DAL.DBContext;
using StorekeeperAssistant.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;

namespace StorekeeperAssistant.DAL.Repositories.Implementation
{
    internal class MovingDetailRepository : BaseRepository<MovingDetail>, IMovingDetailRepository
    {
        private readonly IAppDBContext _context;

        public MovingDetailRepository(IAppDBContext context)
            : base(context.MovingDetails)
        {
            _context = context;
        }

        public async Task<List<MovingDetail>> GetByMovingIdAsync(int movingId)
        {
            return await DbSet.Where(x=>x.MovingId == movingId).ToListAsync();
        }
    }
}
