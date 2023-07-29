using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseCreation
{
    public class ShoppingList
    {
        public int ShoppingListId { get; set; }
        public string ShoppingListName { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
        public ICollection<ShoppingListDetails> Products { get; set; }

        public ShoppingList()
        {
            Products = new HashSet<ShoppingListDetails>();
        }
    }
}
