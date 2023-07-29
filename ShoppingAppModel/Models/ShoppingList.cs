using System;
using System.Collections.Generic;

namespace ShoppingAppModel.Models;

public partial class ShoppingList
{
    public int ShoppingListId { get; set; }

    public string ShoppingListName { get; set; } = null!;

    public int UserId { get; set; }

    public virtual ICollection<ShoppingListDetail> ShoppingListDetails { get; set; } = new List<ShoppingListDetail>();

    public virtual User User { get; set; } = null!;
}
