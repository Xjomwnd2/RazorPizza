using Microsoft.EntityFrameworkCore;
using RazorPizza.Data;
using RazorPizza.Models;

namespace RazorPizza.Services;

public class ToppingService : IToppingService
{
    private readonly PizzaDbContext _context;

    public ToppingService(PizzaDbContext context)
    {
        _context = context;
    }

    public async Task<List<Topping>> GetAllToppingsAsync()
    {
        return await _context.Toppings.ToListAsync();
    }

    public async Task<Topping?> GetToppingByIdAsync(int toppingId)
    {
        return await _context.Toppings.FindAsync(toppingId);
    }
}