using Microsoft.AspNetCore.Mvc;
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

    }
}
