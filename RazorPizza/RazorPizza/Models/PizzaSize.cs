namespace RazorPizza.Models;

public static class PizzaSizes
{
    public static Dictionary<string, decimal> SizePriceMultipliers = new()
    {
        { "Small", 0.75m },
        { "Medium", 1.0m },
        { "Large", 1.35m },
        { "ExtraLarge", 1.65m }
    };
    
    public static Dictionary<string, string> SizeDescriptions = new()
    {
        { "Small", "10\" - 6 slices" },
        { "Medium", "12\" - 8 slices" },
        { "Large", "14\" - 10 slices" },
        { "ExtraLarge", "16\" - 12 slices" }
    };
}