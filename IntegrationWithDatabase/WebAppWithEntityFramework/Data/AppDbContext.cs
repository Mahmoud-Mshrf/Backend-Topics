using Microsoft.EntityFrameworkCore;
using WebAppWithEntityFramework.Models;

namespace WebAppWithEntityFramework.Data;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<Product> Products => Set<Product>();
    public DbSet<ProductReview> ProductsReviews => Set<ProductReview>();
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
    }
}