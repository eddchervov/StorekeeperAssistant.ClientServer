using StorekeeperAssistant.DAL.DBContext;
using StorekeeperAssistant.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using StorekeeperAssistant.DAL.Models;
using Microsoft.EntityFrameworkCore;

namespace StorekeeperAssistant.DAL.Repositories.Implementation
{
    internal class MovingInventoryItemsRepository : BaseRepository<MovingInventoryItems>, IMovingInventoryItemsRepository
    {
        private readonly IAppDBContext _context;

        public MovingInventoryItemsRepository(IAppDBContext context)
            : base(context.MovingInventoryItems)
        {
            _context = context;
        }

        public async Task<List<InventoryItemsDALModel>> GetInventoryItemsByMovingIdAsync(int movingId)
        {
            var q = from mii in _context.MovingInventoryItems
                    join ii in _context.InventoryItems on mii.InventoryItemId equals ii.Id
                    join n in _context.Nomenclatures on ii.NomenclatureId equals n.Id
                    where mii.MovingId == movingId
                    select new InventoryItemsDALModel
                    {
                        Id = ii.Id,
                        NomenclatureDALModel = new NomenclatureDALModel 
                        {
                            Id = n.Id,
                            Name = n.Name
                        }
                    };

            return await q.ToListAsync();
        }
    }
}
