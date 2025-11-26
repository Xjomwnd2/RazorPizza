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
    }
}
