using System;
using System.Collections.Generic;

namespace CoffeeShopManagement.Models.Models;

public partial class Product
{
    public Guid Id { get; set; }

    public string ProductName { get; set; } = null!;

    public Guid CategotyId { get; set; }

    public decimal Price { get; set; }

    public int Quantity { get; set; }

    public string? Thumbnail { get; set; }

    public int Status { get; set; }

    public virtual Category Categoty { get; set; } = null!;

    public virtual ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();
}
