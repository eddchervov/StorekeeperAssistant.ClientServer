using Microsoft.EntityFrameworkCore;
using StorekeeperAssistant.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace StorekeeperAssistant.DAL.DBContext
{
    public interface IAppDBContext : IBaseDbContext
    {
        DbSet<Warehouse> Warehouses { get; set; }
        DbSet<Nomenclature> Nomenclatures { get; set; }
        DbSet<InventoryItem> InventoryItems { get; set; }
        DbSet<Moving> Movings { get; set; }
        DbSet<MovingInventoryItems> MovingInventoryItems { get; set; }
    }
}
