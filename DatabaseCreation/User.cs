﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseCreation
{
    public class User
    {
        public int UserId { get; set; }
        public string Name { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Password { get; set; } = null!;
        public bool IsAdmin { get; set; } = false;
        public ICollection<ShoppingList> ShoppingLists { get; set; } = new HashSet<ShoppingList>();
    }
}
