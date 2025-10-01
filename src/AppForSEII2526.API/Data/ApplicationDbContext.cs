using AppForSEII2526.API.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Numerics;

namespace AppForSEII2526.API.Data;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : IdentityDbContext<ApplicationUser>(options) {
    public DbSet<Purchase> Purchases { get; set; }
    public DbSet<PaymentMethod> PaymentMethods { get; set; }
    public DbSet<TypeItem> TypeItems { get; set; }
    public DbSet<Class> Classes { get; set; }
    public DbSet<PlanItem> PlanItems { get; set; }
    public DbSet<Plan> Plans { get; set; }
    public DbSet<PurchaseItem> PurchaseItems { get; set; }
    public DbSet<ApplicationUser> ApplicationUsers { get; set; }

    public DbSet<Item> Items { get; set; }  
    public DbSet<Brand> Brands { get; set; }
    public DbSet<Bizum> Bizums { get; set; }
    public DbSet<CreditCard> CreditCards { get; set; }
    public DbSet<PayPal> PayPals { get; set; }

    protected override void OnModelCreating(ModelBuilder builder) {
        base.OnModelCreating(builder);


        builder.Entity<Brand>()
            .HasMany(b => b.Items)
            .WithOne(c => c.Brand)
            .OnDelete(DeleteBehavior.NoAction);

        builder.Entity<TypeItem>()
            .HasMany(b => b.Items)
            .WithOne(c => c.TypeItem)
            .OnDelete(DeleteBehavior.NoAction);

    }


}

