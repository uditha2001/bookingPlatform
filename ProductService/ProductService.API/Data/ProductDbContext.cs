using Microsoft.EntityFrameworkCore;
using ProductService.API.Models.Entities;

namespace ProductService.API.Data
{
    public class ProductDbContext: DbContext
    {
        public ProductDbContext(DbContextOptions options) : base(options)
        {

        }
        public DbSet<ProductEntity> Products { get; set; }
        public DbSet<ProductContentEntity> Content { get; set; }
        public DbSet<ProductAttributesEntity> productAttribute { get; set; }



        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
           
            modelBuilder.Entity<ProductEntity>()
           .HasMany(p => p.Attributes)
           .WithOne(a => a.ProductEntity)
           .HasForeignKey(a => a.ProductId)
           .OnDelete(DeleteBehavior.Cascade);


            modelBuilder.Entity<ProductEntity>()
                .HasMany(p => p.Contents)
                .WithOne(c => c.ProductEntity)
                .HasForeignKey(c => c.ProductId)
                .OnDelete(DeleteBehavior.Cascade);

        }



    }
}
