using RazorPizza.Models;

namespace RazorPizza.Data;

public static class DbInitializer
{
    public static void Initialize(PizzaDbContext context)
    {
        // Check if database has pizzas already
        if (context.Pizzas.Any())
        {
            return; // Database has been seeded
        }

        var pizzas = new Pizza[]
        {
            new Pizza
            {
                Name = "Margherita",
                Description = "Classic pizza with tomato sauce, fresh mozzarella, and basil",
                BasePrice = 12.99m,
                ImageUrl = "https://images.unsplash.com/photo-1574071318508-1cdbab80d002?w=500",
                Category = "Specialty",
                IsAvailable = true,
                CreatedDate = DateTime.Now
            },
            new Pizza
            {
                Name = "Pepperoni",
                Description = "Traditional pepperoni with mozzarella cheese and tomato sauce",
                BasePrice = 14.99m,
                ImageUrl = "https://images.unsplash.com/photo-1628840042765-356cda07504e?w=500",
                Category = "Specialty",
                IsAvailable = true,
                CreatedDate = DateTime.Now
            },
            new Pizza
            {
                Name = "Vegetarian Supreme",
                Description = "Bell peppers, onions, mushrooms, olives, and tomatoes",
                BasePrice = 13.99m,
                ImageUrl = "https://images.unsplash.com/photo-1511689660979-10d2b1aada49?w=500",
                Category = "Vegetarian",
                IsAvailable = true,
                CreatedDate = DateTime.Now
            },
            new Pizza
            {
                Name = "BBQ Chicken",
                Description = "Grilled chicken, BBQ sauce, red onions, and cilantro",
                BasePrice = 15.99m,
                ImageUrl = "https://images.unsplash.com/photo-1565299624946-b28f40a0ae38?w=500",
                Category = "Specialty",
                IsAvailable = true,
                CreatedDate = DateTime.Now
            },
            new Pizza
            {
                Name = "Hawaiian",
                Description = "Ham, pineapple, and mozzarella cheese",
                BasePrice = 14.49m,
                ImageUrl = "https://images.unsplash.com/photo-1565299507177-b0ac66763828?w=500",
                Category = "Specialty",
                IsAvailable = true,
                CreatedDate = DateTime.Now
            },
            new Pizza
            {
                Name = "Four Cheese",
                Description = "Mozzarella, parmesan, gorgonzola, and ricotta",
                BasePrice = 16.99m,
                ImageUrl = "https://images.unsplash.com/photo-1513104890138-7c749659a591?w=500",
                Category = "Vegetarian",
                IsAvailable = true,
                CreatedDate = DateTime.Now
            }
        };

        context.Pizzas.AddRange(pizzas);
        context.SaveChanges();
    }
}