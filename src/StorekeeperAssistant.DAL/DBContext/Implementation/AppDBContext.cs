using Microsoft.EntityFrameworkCore;
using StorekeeperAssistant.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace StorekeeperAssistant.DAL.DBContext.Implementation
{
    internal class AppDBContext : BaseDbContext, IAppDBContext
    {
        public AppDBContext(DbContextOptions options) : base(options)
        {
            Database.EnsureDeleted();
            Database.EnsureCreated();
        }

        public DbSet<Warehouse> Warehouses { get; set; }
        public DbSet<Nomenclature> Nomenclatures { get; set; }
        public DbSet<InventoryItem> InventoryItems { get; set; }
        public DbSet<Moving> Movings { get; set; }
        public DbSet<MovingInventoryItems> MovingInventoryItems { get; set; }

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

            builder.Entity<InventoryItem>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.HasOne<Nomenclature>().WithMany().HasForeignKey(p => p.NomenclatureId);
                entity.HasOne<Warehouse>().WithMany().HasForeignKey(p => p.WarehouseId);
            });

            builder.Entity<Moving>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.HasOne<Warehouse>().WithMany().HasForeignKey(p => p.ArrivalWarehouseId);
                entity.HasOne<Warehouse>().WithMany().HasForeignKey(p => p.DepartureWarehouseId);
            });

            builder.Entity<MovingInventoryItems>(entity =>
            {
                entity.HasOne<Moving>().WithMany().HasForeignKey(p => p.MovingId);
                entity.HasOne<InventoryItem>().WithMany().HasForeignKey(p => p.InventoryItemId);
            });
        }

        private void CreateTestData(ref ModelBuilder builder)
        {
            
        }
    }
}
