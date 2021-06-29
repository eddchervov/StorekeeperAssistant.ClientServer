using Microsoft.EntityFrameworkCore;
using StorekeeperAssistant.DAL.Entities;

namespace StorekeeperAssistant.DAL.DBContext.Implementation
{
    public class AppDBContext : BaseDbContext, IAppDBContext
    {
        public AppDBContext(DbContextOptions options) : base(options)
        {
            //Database.EnsureDeleted();
            //Database.EnsureCreated();
        }

        public DbSet<Warehouse> Warehouses { get; set; }
        public DbSet<InventoryItem> InventoryItems { get; set; }
        public DbSet<Moving> Movings { get; set; }
        public DbSet<MovingDetail> MovingDetails { get; set; }
        public DbSet<WarehouseInventoryItem> WarehouseInventoryItems { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            CreateTestData(ref builder);

            builder.Entity<Warehouse>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.HasIndex(e => e.Name).IsUnique();
                entity.Property(e => e.Name).IsRequired();
            });

            builder.Entity<InventoryItem>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.HasIndex(e => e.Name).IsUnique();
                entity.Property(e => e.Name).IsRequired();
            });

            builder.Entity<Moving>(entity =>
            {
                entity.HasKey(e => e.Id);
            });

            builder.Entity<MovingDetail>(entity =>
            {
                entity.HasKey(e => e.Id);
            });

            builder.Entity<WarehouseInventoryItem>(entity =>
            {
                entity.HasKey(e => e.Id);
            });
            
        }

        private void CreateTestData(ref ModelBuilder builder)
        {
            builder.Entity<Warehouse>().HasData(
            new Warehouse[]
            {
                new Warehouse
                {
                    Id = 1,
                    Name = "Склад А"
                },
                new Warehouse
                {
                    Id = 2,
                    Name = "Склад Б"
                },
                new Warehouse
                {
                    Id = 3,
                    Name = "Склад В"
                }
            });

            builder.Entity<InventoryItem>().HasData(
            new InventoryItem[]
            {
                new InventoryItem
                {
                    Id = 1,
                    Name = "Номенклатура А"
                },
                new InventoryItem
                {
                    Id = 2,
                    Name = "Номенклатура Б"
                },
                new InventoryItem
                {
                    Id = 3,
                    Name = "Номенклатура В"
                },
                new InventoryItem
                {
                    Id = 4,
                    Name = "Номенклатура Г"
                },
                new InventoryItem
                {
                    Id = 5,
                    Name = "Номенклатура Д"
                },
                new InventoryItem
                {
                    Id = 6,
                    Name = "Номенклатура Е"
                },
                new InventoryItem
                {
                    Id = 7,
                    Name = "Номенклатура Ж"
                },
            });
        }
    }
}
