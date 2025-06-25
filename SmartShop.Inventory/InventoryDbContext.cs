using Microsoft.EntityFrameworkCore;

namespace SmartShop.Inventory
{
    public class InventoryDbContext : DbContext
    {
        public DbSet<Product> Products { get; set; }
        public DbSet<Store> Stores { get; set; }
        public DbSet<Inventory> Inventories { get; set; }
        public DbSet<Supplier> Suppliers { get; set; }
        public DbSet<Sale> Sales { get; set; }
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Product configuration
            modelBuilder.Entity<Product>(entity => 
            {
                entity.Property(p => p.Name).HasMaxLength(100).IsRequired();
                entity.Property(p => p.Price).HasColumnType("decimal(10,2)").IsRequired();
                entity.Property(p => p.Category).HasMaxLength(50).IsRequired();
            });
            
            // Store configuration
            modelBuilder.Entity<Store>(entity =>
            {
                entity.Property(s => s.Name).HasMaxLength(100).IsRequired();
                entity.Property(s => s.Location).HasMaxLength(200).IsRequired();
            });
            
            // Inventory configuration
            modelBuilder.Entity<Inventory>(entity =>
            {
                entity.HasOne(i => i.Product)
                    .WithMany()
                    .HasForeignKey(i => i.ProductId);
                    
                entity.HasOne(i => i.Store)
                    .WithMany()
                    .HasForeignKey(i => i.StoreId);
            });
            
            // Product-Supplier many-to-many
            modelBuilder.Entity<ProductSupplier>()
                .HasKey(ps => new { ps.ProductId, ps.SupplierId });
                
            modelBuilder.Entity<ProductSupplier>()
                .HasOne(ps => ps.Product)
                .WithMany(p => p.ProductSuppliers)
                .HasForeignKey(ps => ps.ProductId);
                
            modelBuilder.Entity<ProductSupplier>()
                .HasOne(ps => ps.Supplier)
                .WithMany(s => s.ProductSuppliers)
                .HasForeignKey(ps => ps.SupplierId);
            
            // Sale configuration
            modelBuilder.Entity<Sale>(entity =>
            {
                entity.Property(s => s.SalePrice).HasColumnType("decimal(10,2)");
                
                entity.HasOne(s => s.Product)
                    .WithMany()
                    .HasForeignKey(s => s.ProductId);
                    
                entity.HasOne(s => s.Store)
                    .WithMany()
                    .HasForeignKey(s => s.StoreId);
            });
        }
        
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=SmartShopInventory.db");
        }
    }
}
