using RazorPizza.Models;

namespace RazorPizza.Services;

public interface IOrderService
{
    Task<List<Order>> GetAllOrdersAsync();
    Task<Order?> GetOrderByIdAsync(int orderId);
    Task<Order> CreateOrderAsync(Order order);
    Task UpdateOrderStatusAsync(int orderId, string status);
}