using RazorPizza.Models;

namespace RazorPizza.Services;

public interface IPizzaService
{
    Task<List<Pizza>> GetAllPizzasAsync();
    Task<Pizza?> GetPizzaByIdAsync(int pizzaId);
    Task<Pizza> CreatePizzaAsync(Pizza pizza);
    Task UpdatePizzaAsync(Pizza pizza);
    Task DeletePizzaAsync(int pizzaId);
}