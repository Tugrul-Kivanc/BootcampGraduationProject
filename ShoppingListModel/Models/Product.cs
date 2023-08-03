using System;
using System.Collections.Generic;

namespace ShoppingListModel.Models;

public partial class Product
{
    public int ProductId { get; set; }

    public string Name { get; set; } = null!;

    // https://imgur.com/a/NSPxANy
    public string Image { get; set; } = null!;

    public int CategoryId { get; set; }

    public virtual Category Category { get; set; } = null!;

    public virtual ICollection<ShoppingListDetail> ShoppingListDetails { get; set; } = new List<ShoppingListDetail>();
}
