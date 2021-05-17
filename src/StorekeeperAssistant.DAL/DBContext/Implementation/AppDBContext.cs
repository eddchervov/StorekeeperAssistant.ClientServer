using Microsoft.EntityFrameworkCore;
using StorekeeperAssistant.DAL.Entities;

namespace StorekeeperAssistant.DAL.DBContext.Implementation
{
    internal class AppDBContext : BaseDbContext, IAppDBContext
    {
        public AppDBContext(DbContextOptions options) : base(options)
        {
            //Database.EnsureDeleted();
            Database.EnsureCreated();
        }

        public DbSet<Warehouse> Warehouses { get; set; }
        public DbSet<Nomenclature> Nomenclatures { get; set; }
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

            builder.Entity<Nomenclature>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.HasIndex(e => e.Name).IsUnique();
                entity.Property(e => e.Name).IsRequired();
            });

            builder.Entity<Moving>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.HasOne<Warehouse>().WithMany().HasForeignKey(p => p.ArrivalWarehouseId).OnDelete(DeleteBehavior.Restrict);
                entity.HasOne<Warehouse>().WithMany().HasForeignKey(p => p.DepartureWarehouseId).OnDelete(DeleteBehavior.Restrict);
            });

            builder.Entity<MovingDetail>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.HasOne<Moving>().WithMany().HasForeignKey(p => p.MovingId).OnDelete(DeleteBehavior.Restrict);
                entity.HasOne<Nomenclature>().WithMany().HasForeignKey(p => p.NomenclatureId).OnDelete(DeleteBehavior.Restrict);
            });

            builder.Entity<WarehouseInventoryItem>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.HasOne<Nomenclature>().WithMany().HasForeignKey(p => p.NomenclatureId);
                entity.HasOne<Warehouse>().WithMany().HasForeignKey(p => p.WarehouseId);
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

            builder.Entity<Nomenclature>().HasData(
            new Nomenclature[]
            {
                new Nomenclature
                {
                    Id = 1,
                    Name = "Номенклатура А"
                },
                new Nomenclature
                {
                    Id = 2,
                    Name = "Номенклатура Б"
                },
                new Nomenclature
                {
                    Id = 3,
                    Name = "Номенклатура В"
                },
                new Nomenclature
                {
                    Id = 4,
                    Name = "Номенклатура Г"
                },
                new Nomenclature
                {
                    Id = 5,
                    Name = "Номенклатура Д"
                },
                new Nomenclature
                {
                    Id = 6,
                    Name = "Номенклатура Е"
                },
                new Nomenclature
                {
                    Id = 7,
                    Name = "Номенклатура Ж"
                },
            });
        }
    }
}
