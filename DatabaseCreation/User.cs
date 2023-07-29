using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseCreation
{
    public class User
    {
        public int UserId { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public ICollection<ShoppingList> ShoppingLists { get; set; }

        public User()
        {
            ShoppingLists = new HashSet<ShoppingList>();
        }
    }
}
