using System;
using System.Collections.Generic;

namespace CoffeeShopManagement.Models.Models;

public partial class Category
{
    public Guid Id { get; set; }

    public string CategoryName { get; set; } = null!;

    public virtual ICollection<Product> Products { get; set; } = new List<Product>();
}
