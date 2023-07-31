using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseCreation
{
    public class ShoppingListDetails
    {
        public int ShoppingListId { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public string? Note { get; set; }
        public ShoppingList ShoppingList { get; set; } = null!;
        public Product Product { get; set; } = null!;
    }
}