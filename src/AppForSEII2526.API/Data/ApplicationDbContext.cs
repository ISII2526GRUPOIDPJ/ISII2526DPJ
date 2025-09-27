using AppForSEII2526.API.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Numerics;

namespace AppForSEII2526.API.Data;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : IdentityDbContext<ApplicationUser>(options) {
    public DbSet<PaymentMethod> PaymentMethods { get; set; }
    public DbSet<TypeItem> TypeItems { get; set; }
    public DbSet<Class> Classes { get; set; }
    public DbSet<PlanItem> PlanItems { get; set; }
    public DbSet<Plan> Plans { get; set; }
    public DbSet<PurchaseItem> PurchaseItems { get; set; }
}
