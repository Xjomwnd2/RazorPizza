using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RazorPizza.Services;
using RazorPizza.Models;

namespace RazorPizza.Pages
{
    public class AdminModel : PageModel
    {
        private readonly IPizzaService _pizzaService;
        private readonly IOrderService _orderService;
        private readonly IToppingService _toppingService;

        public AdminModel(
            IPizzaService pizzaService, 
            IOrderService orderService, 
            IToppingService toppingService)
        {
            _pizzaService = pizzaService;
            _orderService = orderService;
            _toppingService = toppingService;
        }

        public List<Pizza> Pizzas { get; set; } = new List<Pizza>();
        public List<Order> Orders { get; set; } = new List<Order>();
        public List<Topping> Toppings { get; set; } = new List<Topping>();

        public async Task OnGetAsync()
        {
            await LoadData();
        }

        public async Task<IActionResult> OnPostDeletePizzaAsync(int id)
        {
            await _pizzaService.DeletePizzaAsync(id);
            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostDeleteToppingAsync(int id)
        {
            await _toppingService.DeleteToppingAsync(id);
            return RedirectToPage();
        }

        private async Task LoadData()
        {
            Pizzas = await _pizzaService.GetAllPizzasAsync();
            Orders = await _orderService.GetAllOrdersAsync();
            Toppings = await _toppingService.GetAllToppingsAsync();
        }
    }
}