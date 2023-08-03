using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShoppingListApp.ViewModels;
using ShoppingListModel.Models;

namespace ShoppingListApp.Controllers
{
    public class ShoppingListController : ControllerBase
    {
        public IActionResult List(int id) // Takes User Id
        {
            var shoppingLists = context.ShoppingLists.Where(a => a.UserId == id);
            return View(shoppingLists);
        }

        public IActionResult ListDetails(int id) // Takes Shopping List Id
        {
            var query = from l in context.ShoppingLists
                        join d in context.ShoppingListDetails
                        on l.ShoppingListId equals d.ShoppingListId
                        into listDetails
                        from ld in listDetails.DefaultIfEmpty()
                        where ld.ShoppingListId == id
                        select new ShoppingListViewModel
                        {
                            Name = ld.Product.Name,
                            Quantity = ld.Quantity,
                            Notes = ld.Note
                        };

            return View(query.ToList());
        }

        [HttpGet]
        public IActionResult CreateList()
        {
            return View();
        }

        [HttpPost]
        public IActionResult CreateList(ShoppingList list)
        {

            return View();
        }

        public IActionResult EditList(int id) // Takes Shopping List Id
        {
            var shoppingListDetails = context.ShoppingListDetails.Include(a => a.Product)
                .Where(b => b.ShoppingListId == id)
                .ToList();

            return View(shoppingListDetails);
        }

        public IActionResult DeleteList()
        {
            return View();
        }

        public IActionResult Shop()
        {
            return View();
        }
    }
}
