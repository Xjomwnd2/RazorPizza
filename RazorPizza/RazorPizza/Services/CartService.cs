// Services/CartService.cs
public class CartService : ICartService
{
    private readonly IPizzaService _pizzaService;
    private readonly IToppingService _toppingService;
    private readonly IPromoCodeService _promoCodeService;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly ApplicationDbContext _context; // For logged-in users
    
    // Session key for cart storage
    private const string CartSessionKey = "ShoppingCart";

    public CartService(
        IPizzaService pizzaService,
        IToppingService toppingService,
        IPromoCodeService promoCodeService,
        IHttpContextAccessor httpContextAccessor,
        ApplicationDbContext context)
    {
        _pizzaService = pizzaService;
        _toppingService = toppingService;
        _promoCodeService = promoCodeService;
        _httpContextAccessor = httpContextAccessor;
        _context = context;
    }

    public async Task<bool> AddToCart(int pizzaId, List<int> toppingIds, int quantity = 1)
    {
        try
        {
            // 1. Validate the pizza exists
            var pizza = await _pizzaService.GetPizzaById(pizzaId);
            if (pizza == null)
                return false;

            // 2. Validate and get toppings
            var toppings = await _toppingService.GetToppingsByIds(toppingIds);
            
            // 3. Calculate the total price for this item
            decimal itemPrice = pizza.Price;
            foreach (var topping in toppings)
            {
                itemPrice += topping.Price;
            }

            // 4. Get the current cart
            var cart = await GetCart();

            // 5. Check if item already exists in cart (same pizza + same toppings)
            var existingItem = cart.Items.FirstOrDefault(i => 
                i.PizzaId == pizzaId && 
                i.SelectedToppingIds.OrderBy(t => t).SequenceEqual(toppingIds.OrderBy(t => t)));

            if (existingItem != null)
            {
                // Update quantity of existing item
                existingItem.Quantity += quantity;
                existingItem.TotalPrice = itemPrice * existingItem.Quantity;
            }
            else
            {
                // Add new item to cart
                var newItem = new CartItem
                {
                    PizzaId = pizzaId,
                    PizzaName = pizza.Name,
                    BasePrice = pizza.Price,
                    Quantity = quantity,
                    SelectedToppingIds = toppingIds,
                    TotalPrice = itemPrice * quantity
                };
                cart.Items.Add(newItem);
            }

            // 6. Recalculate cart totals
            cart.Subtotal = cart.Items.Sum(i => i.TotalPrice);
            cart.Total = cart.Subtotal - cart.Discount;

            // 7. Save the cart
            await SaveCart(cart);

            return true;
        }
        catch (Exception ex)
        {
            // Log the exception
            // _logger.LogError(ex, "Error adding item to cart");
            return false;
        }
    }

    public async Task<bool> RemoveFromCart(int pizzaId)
    {
        var cart = await GetCart();
        var itemToRemove = cart.Items.FirstOrDefault(i => i.PizzaId == pizzaId);
        
        if (itemToRemove == null)
            return false;

        cart.Items.Remove(itemToRemove);
        
        // Recalculate totals
        cart.Subtotal = cart.Items.Sum(i => i.TotalPrice);
        cart.Total = cart.Subtotal - cart.Discount;
        
        await SaveCart(cart);
        return true;
    }

    public async Task<bool> UpdateQuantity(int pizzaId, int newQuantity)
    {
        if (newQuantity <= 0)
            return await RemoveFromCart(pizzaId);

        var cart = await GetCart();
        var item = cart.Items.FirstOrDefault(i => i.PizzaId == pizzaId);
        
        if (item == null)
            return false;

        item.Quantity = newQuantity;
        item.TotalPrice = (item.TotalPrice / item.Quantity) * newQuantity;
        
        // Recalculate totals
        cart.Subtotal = cart.Items.Sum(i => i.TotalPrice);
        cart.Total = cart.Subtotal - cart.Discount;
        
        await SaveCart(cart);
        return true;
    }

    public async Task<CartModel> GetCart()
    {
        var session = _httpContextAccessor.HttpContext.Session;
        var user = _httpContextAccessor.HttpContext.User;

        // For logged-in users, retrieve from database
        if (user.Identity.IsAuthenticated)
        {
            var userId = user.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            var dbCart = await _context.Carts
                .Include(c => c.Items)
                .FirstOrDefaultAsync(c => c.UserId == userId);

            if (dbCart != null)
            {
                return MapDbCartToModel(dbCart);
            }
        }

        // For anonymous users, use session
        var cartJson = session.GetString(CartSessionKey);
        if (string.IsNullOrEmpty(cartJson))
        {
            return new CartModel();
        }

        return JsonSerializer.Deserialize<CartModel>(cartJson);
    }

    public async Task<decimal> CalculateTotal(string promoCode = null)
    {
        var cart = await GetCart();
        decimal total = cart.Subtotal;

        if (!string.IsNullOrEmpty(promoCode))
        {
            var discount = await _promoCodeService.GetDiscount(promoCode, total);
            total -= discount;
            cart.Discount = discount;
        }

        return total;
    }

    public async Task<bool> ApplyPromoCode(string promoCode)
    {
        var cart = await GetCart();
        var discount = await _promoCodeService.GetDiscount(promoCode, cart.Subtotal);
        
        if (discount > 0)
        {
            cart.PromoCode = promoCode;
            cart.Discount = discount;
            cart.Total = cart.Subtotal - discount;
            await SaveCart(cart);
            return true;
        }

        return false;
    }

    public async Task ClearCart()
    {
        var session = _httpContextAccessor.HttpContext.Session;
        var user = _httpContextAccessor.HttpContext.User;

        // Clear from database if logged in
        if (user.Identity.IsAuthenticated)
        {
            var userId = user.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            var dbCart = await _context.Carts
                .Include(c => c.Items)
                .FirstOrDefaultAsync(c => c.UserId == userId);

            if (dbCart != null)
            {
                _context.CartItems.RemoveRange(dbCart.Items);
                _context.Carts.Remove(dbCart);
                await _context.SaveChangesAsync();
            }
        }

        // Clear from session
        session.Remove(CartSessionKey);
    }

    private async Task SaveCart(CartModel cart)
    {
        var session = _httpContextAccessor.HttpContext.Session;
        var user = _httpContextAccessor.HttpContext.User;

        // Save to database if logged in
        if (user.Identity.IsAuthenticated)
        {
            var userId = user.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            var dbCart = await _context.Carts
                .Include(c => c.Items)
                .FirstOrDefaultAsync(c => c.UserId == userId);

            if (dbCart == null)
            {
                dbCart = new Cart { UserId = userId };
                _context.Carts.Add(dbCart);
            }

            // Update cart items
            _context.CartItems.RemoveRange(dbCart.Items);
            dbCart.Items = MapModelToDbCartItems(cart, dbCart.Id);
            
            await _context.SaveChangesAsync();
        }

        // Also save to session
        var cartJson = JsonSerializer.Serialize(cart);
        session.SetString(CartSessionKey, cartJson);
    }

    private CartModel MapDbCartToModel(Cart dbCart)
    {
        // Implementation to map database cart to model
        return new CartModel
        {
            Items = dbCart.Items.Select(i => new CartItem
            {
                PizzaId = i.PizzaId,
                PizzaName = i.PizzaName,
                BasePrice = i.BasePrice,
                Quantity = i.Quantity,
                SelectedToppingIds = JsonSerializer.Deserialize<List<int>>(i.ToppingsJson),
                TotalPrice = i.TotalPrice
            }).ToList(),
            Subtotal = dbCart.Subtotal,
            Discount = dbCart.Discount,
            Total = dbCart.Total,
            PromoCode = dbCart.PromoCode
        };
    }

    private List<CartItem> MapModelToDbCartItems(CartModel cart, int cartId)
    {
        // Implementation to map model to database cart items
        return cart.Items.Select(i => new CartItemEntity
        {
            CartId = cartId,
            PizzaId = i.PizzaId,
            PizzaName = i.PizzaName,
            BasePrice = i.BasePrice,
            Quantity = i.Quantity,
            ToppingsJson = JsonSerializer.Serialize(i.SelectedToppingIds),
            TotalPrice = i.TotalPrice
        }).ToList();
    }
}