using Microsoft.AspNetCore.Mvc;

namespace ShoppingListApp.Controllers
{
    public class AdminController : ControllerBase
    {
        public IActionResult AdminPanel()
        {
            return View();
        }

        public IActionResult AddCategory()
        {
            return View();
        }

        public IActionResult EditCategory()
        {
            return View();
        }

        public IActionResult DeleteCategory()
        {
            return View();
        }

        public IActionResult AddProduct()
        {
            return View();
        }

        public IActionResult EditProduct()
        {
            return View();
        }

        public IActionResult RemoveProduct()
        {
            return View();
        }
    }
}
