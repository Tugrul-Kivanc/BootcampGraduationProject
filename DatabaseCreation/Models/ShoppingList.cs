using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseCreation.Models
{
    public class ShoppingList
    {
        public int ShoppingListId { get; set; }
        public string ShoppingListName { get; set; } = null!;
        public int UserId { get; set; }
        public User User { get; set; } = null!;
        public ICollection<ShoppingListDetails> Products { get; set; } = new HashSet<ShoppingListDetails>();
    }
}
