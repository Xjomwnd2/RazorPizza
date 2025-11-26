namespace RazorPizza.Services;

public interface IToppingService
{
    Task<List<Topping>> GetAllToppingsAsync();
    Task<Topping?> GetToppingByIdAsync(int toppingId);
}