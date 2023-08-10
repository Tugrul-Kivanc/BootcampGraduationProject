using Microsoft.AspNetCore.Mvc;
using ShoppingListApp.Extensions;
using ShoppingListModel.Models;

namespace ShoppingListApp.Controllers
{
    public class ControllerBase : Controller
    {
        protected readonly ShoppingListAppDbContext context;
        public ControllerBase()
        {
            context = new ShoppingListAppDbContext();
        }

        protected bool TryGetUserFromSession(out User? user)
        {
            user = HttpContext.Session.GetObject<User>("User");
            return user == null ? false : true;
        }
    }
}
