cat > Services/IToppingService.cs << 'EOF'
using RazorPizza.Models;

namespace RazorPizza.Services;

public interface IToppingService
{
    Task<List<Topping>> GetAllToppingsAsync();
    Task<Topping?> GetToppingByIdAsync(int toppingId);
}
EOF