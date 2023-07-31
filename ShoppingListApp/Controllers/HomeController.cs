using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShoppingListModel.Models;
using System.Diagnostics;

namespace ShoppingListApp.Controllers
{
    public class HomeController : Controller
    {
        ShoppingListAppDbContext context;
        public HomeController()
        {
            context = new ShoppingListAppDbContext();
        }

        public IActionResult ShoppingLists(int id) // Takes User Id
        {
            var shoppingLists = context.ShoppingLists.Where(b => b.UserId == id);
            return View(shoppingLists);
        }
        public IActionResult Login()
        {
            return View();
        }
        public IActionResult Register()
        {
            return View();
        }
        public IActionResult AdminPanel()
        {
            return View();
        }
        public IActionResult CreateList()
        {
            return View();
        }
        public IActionResult Shop()
        {
            return View();
        }
        public IActionResult EditShoppingList(int id) // Takes Shopping List Id
        {
            var shoppingListDetails = context.ShoppingListDetails.Include(a => a.Product).Where(b => b.ShoppingListId == id).ToList();
            return View(shoppingListDetails);
        }
        public IActionResult DeleteShoppingList()
        {
            return View();
        }
    }
}