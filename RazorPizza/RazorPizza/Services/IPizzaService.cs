// File: IPizzaService.cs
using RazorPizza.Models;

namespace RazorPizza.Services;

public interface IPizzaService
{
    Task<List<Pizza>> GetAllPizzasAsync();
    Task<Pizza?> GetPizzaByIdAsync(int id);
    Task<List<Pizza>> GetPizzasByCategoryAsync(string category);
    Task<Pizza> CreatePizzaAsync(Pizza pizza);
    Task<Pizza> UpdatePizzaAsync(Pizza pizza);
    Task<bool> DeletePizzaAsync(int id);
    Task<List<string>> GetCategoriesAsync();
}