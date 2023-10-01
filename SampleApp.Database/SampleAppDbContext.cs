using Microsoft.EntityFrameworkCore;
using SampleApp.Domain;

namespace SampleApp.Database;

public class SampleAppDbContext : DbContext
{
    public SampleAppDbContext(DbContextOptions<SampleAppDbContext> options) :
        base(options)
    {

    }

    public DbSet<Customer> Customers { get; set; }
    public DbSet<Order> Orders { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Customer>().ToTable("Customer");
        modelBuilder.Entity<Order>().ToTable("Order");

        modelBuilder.Entity<Customer>()
            .HasMany(o => o.Orders)
            .WithOne(o => o.Customer)
            .HasForeignKey(o => o.CustomerId)
            .HasPrincipalKey(o => o.Id);

    }
}