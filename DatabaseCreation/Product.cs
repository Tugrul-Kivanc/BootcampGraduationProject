using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseCreation
{
    public class Product
    {
        public int ProductId { get; set; }
        public string Name { get; set; }
        public string Image { get; set; }
        public int CategoryId { get; set; }
        public Category Category { get; set; }
        public ICollection<ShoppingListDetails> ShoppingLists { get; set; }

        public Product()
        {
            ShoppingLists = new HashSet<ShoppingListDetails>();
        }
    }
}
