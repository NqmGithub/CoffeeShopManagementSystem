using System;
using System.Collections.Generic;

namespace CoffeeShopManagement.Models.Models;

public partial class User
{
    public Guid Id { get; set; }

    public string UserName { get; set; } = null!;

    public string Password { get; set; } = null!;

    public string Email { get; set; } = null!;

    public int Role { get; set; }

    public string PhoneNumber { get; set; } = null!;

    public string? Avatar { get; set; }

    public string? Address { get; set; }

    public int Status { get; set; }

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
}
