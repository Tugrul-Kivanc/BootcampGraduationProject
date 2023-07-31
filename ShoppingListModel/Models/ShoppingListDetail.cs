using System;
using System.Collections.Generic;

namespace ShoppingListModel.Models;

public partial class ShoppingListDetail
{
    public int ShoppingListId { get; set; }

    public int ProductId { get; set; }

    public int Quantity { get; set; }

    public string? Note { get; set; }

    public virtual Product Product { get; set; } = null!;

    public virtual ShoppingList ShoppingList { get; set; } = null!;
}
