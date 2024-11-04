using System;
using System.Collections.Generic;

namespace CoffeeShopManagement.Models.Models;

public partial class OrderDetail
{
    public Guid Id { get; set; }

    public Guid OrderId { get; set; }

    public int Quantity { get; set; }

    public Guid ProductId { get; set; }

    public decimal OrderPrice { get; set; }

    public virtual Order Order { get; set; } = null!;

    public virtual Product Product { get; set; } = null!;
}
