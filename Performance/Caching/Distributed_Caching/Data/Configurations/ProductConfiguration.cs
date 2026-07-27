using Distributed_Caching.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Distributed_Caching.Data.Configurations;

public class ProductConfiguration : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        builder.HasKey(p => p.Id);
        builder.Property(p => p.Id).ValueGeneratedOnAdd();

        builder.ToTable("Products");

        builder.Property(p => p.Name)
               .IsRequired()
               .HasMaxLength(100);

        builder.Property(p => p.Price)
               .IsRequired()
               .HasColumnType("decimal(18,2)");

        builder.HasData(
        [
            new Product { Id = 1, Name = "Soda", Price = 1.99m },
            new Product { Id = 2, Name = "Milk", Price = 3.49m },
            new Product { Id = 3, Name = "Orange Juice", Price = 4.75m },
            new Product { Id = 4, Name = "Bread", Price = 2.49m },
            new Product { Id = 5, Name = "Butter", Price = 3.99m },
            new Product { Id = 6, Name = "Eggs", Price = 2.99m },
            new Product { Id = 7, Name = "Cheese", Price = 5.49m },
            new Product { Id = 8, Name = "Chocolate", Price = 1.99m },
            new Product { Id = 9, Name = "Coffee", Price = 7.99m },
            new Product { Id = 10, Name = "Tea", Price = 4.99m },
            new Product { Id = 11, Name = "Water", Price = 0.99m },
            new Product { Id = 12, Name = "Fruit Mix Juice", Price = 3.99m },
            new Product { Id = 13, Name = "Yogurt", Price = 2.99m },
            new Product { Id = 14, Name = "Cereal", Price = 4.49m },
            new Product { Id = 15, Name = "Rice", Price = 6.99m },
            new Product { Id = 16, Name = "Pasta", Price = 3.49m },
            new Product { Id = 17, Name = "Apple", Price = 0.79m },
            new Product { Id = 18, Name = "Banana", Price = 0.59m },
            new Product { Id = 19, Name = "Orange", Price = 0.99m },
            new Product { Id = 20, Name = "Grapes", Price = 2.99m },
        ]);
    }
}
