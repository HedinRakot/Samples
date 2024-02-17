using Microsoft.EntityFrameworkCore;
using SampleApi.Domain;

namespace SampleApi.Database;

public class SampleAppDbContext : DbContext
{
    public SampleAppDbContext(DbContextOptions<SampleAppDbContext> options) :
        base(options)
    {

    }

    public DbSet<Customer> Customers { get; set; }
    public DbSet<Order> Orders { get; set; }
    public DbSet<Address> Addresses { get; set; }
    public DbSet<CustomerAddress> CustomerAddresses { get; set; }
    public DbSet<Coupon> Coupons { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Customer>().ToTable("Customer");
        modelBuilder.Entity<Order>().ToTable("Order");
        modelBuilder.Entity<Address>().ToTable("Address");
        modelBuilder.Entity<CustomerAddress>().ToTable("CustomersAddress").
            HasKey("CustomersId", "AddressId");

        modelBuilder.Entity<Customer>()
            .HasMany(o => o.Orders)
            .WithOne(o => o.Customer)
            .HasForeignKey(o => o.CustomerId)
            .HasPrincipalKey(o => o.Id);

        //modelBuilder.Entity<Address>()
        //    .HasMany(o => o.Customers)
        //    .WithMany(o => o.Address)
        //    .UsingEntity(
        //        "CustomerAddress",
        //        l => l.HasOne(typeof(Customer)).WithMany().HasForeignKey("CustomerId").HasPrincipalKey(nameof(Customer.Id)),
        //        r => r.HasOne(typeof(Address)).WithMany().HasForeignKey("AddressId").HasPrincipalKey(nameof(Address.Id)),
        //        j => j.HasKey("CustomerId", "AddressId"));


        modelBuilder.Entity<Coupon>().ToTable("Coupon");
        modelBuilder.Entity<Coupon>().Property("Version").IsRowVersion();
    }
}