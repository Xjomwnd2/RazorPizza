namespace RazorPizza.Models;

public class Address
{
    public int AddressId { get; set; }
    public string UserId { get; set; } = string.Empty;
    public string Label { get; set; } = "Home"; // Home, Work, Other
    public string Street { get; set; } = string.Empty;
    public string? Apartment { get; set; }
    public string City { get; set; } = string.Empty;
    public string State { get; set; } = string.Empty;
    public string ZipCode { get; set; } = string.Empty;
    public bool IsDefault { get; set; } = false;
    
    // Navigation property
    public virtual ApplicationUser? User { get; set; }
}