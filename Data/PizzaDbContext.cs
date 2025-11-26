cat > Data/PizzaDbContext.cs << 'EOF'
using Microsoft.EntityFrameworkCore;
using RazorPizza.Models;

namespace RazorPizza.Data;

public class PizzaDbContext : DbContext
{
    public PizzaDbContext(DbContextOptions<PizzaDbContext> options) : base(options)
    {
    }

    public DbSet<Pizza> Pizzas { get; set; }
    public DbSet<Order> Orders { get; set; }
    public DbSet<OrderItem> OrderItems { get; set; }
    public DbSet<PromoCode> PromoCodes { get; set; }
    public DbSet<Topping> Toppings { get; set; }
}
EOF