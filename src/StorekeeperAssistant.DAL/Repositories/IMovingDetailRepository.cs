using StorekeeperAssistant.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace StorekeeperAssistant.DAL.Repositories
{
    public interface IMovingDetailRepository : IBaseRepository<MovingDetail>
    {
        Task<List<MovingDetail>> GetByMovingIdAsync(int movingId);
    }
}
