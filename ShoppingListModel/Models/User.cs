using System;
using System.Collections.Generic;

namespace ShoppingListModel.Models;

public partial class User
{
    public int UserId { get; set; }

    public string Name { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string Password { get; set; } = null!;

    public string ConfirmPassword { get; set; } = null!;

    public bool IsAdmin { get; set; } = false;

    public virtual ICollection<ShoppingList> ShoppingLists { get; set; } = new List<ShoppingList>();
}
