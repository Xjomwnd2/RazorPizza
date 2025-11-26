using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using RazorPizza.Models;

namespace RazorPizza.Data;

public class PizzaDbContext : IdentityDbContext<ApplicationUser>
{
    public PizzaDbContext(DbContextOptions<PizzaDbContext> options)
        : base(options) { }

    public DbSet<Pizza> Pizzas { get; set; }
    public DbSet<Order> Orders { get; set; }
    public DbSet<OrderItem> OrderItems { get; set; }
    public DbSet<PromoCode> PromoCodes { get; set; }
    public DbSet<Topping> Toppings { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        // Ensure IdentityUser.Id is nvarchar(450) for SQL Server
        builder.Entity<ApplicationUser>(b =>
        {
            b.Property(u => u.Id).HasMaxLength(450);
        });

        // Configure decimal precision for all monetary values
        builder.Entity<Order>(entity =>
        {
            entity.Property(e => e.SubTotal).HasPrecision(18, 2);
            entity.Property(e => e.Tax).HasPrecision(18, 2);
            entity.Property(e => e.DeliveryFee).HasPrecision(18, 2);
            entity.Property(e => e.Discount).HasPrecision(18, 2);
            entity.Property(e => e.TotalAmount).HasPrecision(18, 2);
        });

        builder.Entity<OrderItem>(entity =>
        {
            entity.Property(e => e.Price).HasPrecision(18, 2);
        });

        builder.Entity<Pizza>(entity =>
        {
            entity.Property(e => e.BasePrice).HasPrecision(18, 2);
        });

        builder.Entity<PromoCode>(entity =>
        {
            entity.Property(e => e.DiscountPercent).HasPrecision(5, 2);
            entity.Property(e => e.DiscountValue).HasPrecision(18, 2);
            entity.Property(e => e.MaxDiscountAmount).HasPrecision(18, 2);
            entity.Property(e => e.MinOrderAmount).HasPrecision(18, 2);
        });

        builder.Entity<Topping>(entity =>
        {
            entity.Property(e => e.Price).HasPrecision(18, 2);
        });
    }
}