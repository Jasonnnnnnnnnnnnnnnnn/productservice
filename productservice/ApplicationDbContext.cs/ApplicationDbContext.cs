using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using productservice.Model;


public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<Product> Products { get; set; }
    public DbSet<Cart> Carts { get; set; }

    public DbSet<User> Users { get; set; }






    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Product>()
            .ToTable("product");

        modelBuilder.Entity<Cart>()
            .ToTable("cart");




    }






}


