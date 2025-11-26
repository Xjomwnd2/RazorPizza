using Microsoft.EntityFrameworkCore;
using RazorPizza.Data;
using RazorPizza.Models;

namespace RazorPizza.Services;

public class PizzaService : IPizzaService
{
    private readonly PizzaDbContext _context;

    public PizzaService(PizzaDbContext context)
    {
        _context = context;
    }

    public async Task<List<Pizza>> GetAllPizzasAsync()
    {
        return await _context.Pizzas.ToListAsync();
    }

    public async Task<Pizza?> GetPizzaByIdAsync(int pizzaId)
    {
        return await _context.Pizzas.FindAsync(pizzaId);
    }

    public async Task<Pizza> CreatePizzaAsync(Pizza pizza)
    {
        _context.Pizzas.Add(pizza);
        await _context.SaveChangesAsync();
        return pizza;
    }

    public async Task UpdatePizzaAsync(Pizza pizza)
    {
        _context.Pizzas.Update(pizza);
        await _context.SaveChangesAsync();
    }

    public async Task DeletePizzaAsync(int pizzaId)
    {
        var pizza = await _context.Pizzas.FindAsync(pizzaId);
        if (pizza != null)
        {
            _context.Pizzas.Remove(pizza);
            await _context.SaveChangesAsync();
        }
    }
}