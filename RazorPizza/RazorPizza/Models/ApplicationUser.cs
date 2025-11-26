using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace RazorPizza.Models;

public class ApplicationUser : IdentityUser
{
    [MaxLength(450)]
    public override string Id { get; set; } = Guid.NewGuid().ToString();

    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

    // Navigation properties
    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
    public virtual ICollection<Address> Addresses { get; set; } = new List<Address>();
}
