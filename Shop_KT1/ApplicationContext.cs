using Microsoft.EntityFrameworkCore;
using Shop_KT1.models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace Shop_KT1
{
    public class ApplicationContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Material> Materials { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<ProductMaterial> ProductMaterials { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<Production> Productions { get; set; }
        public DbSet<ProductionMaterial> ProductionMaterials { get; set; }
        public DbSet<InventoryAudit> InventoryAudits { get; set; }
        public DbSet<InventoryItem> InventoryItems { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=Shop.db");
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ProductMaterial>()
                .HasKey(pm => pm.ProductMaterialId);

            modelBuilder.Entity<OrderItem>()
                .HasKey(oi => oi.OrderItemID);

            modelBuilder.Entity<ProductionMaterial>()
                .HasKey(pm => pm.ProductionMaterialID);

            modelBuilder.Entity<InventoryItem>()
                .HasKey(ii => ii.InventoryItemID);
        }
    }
}
