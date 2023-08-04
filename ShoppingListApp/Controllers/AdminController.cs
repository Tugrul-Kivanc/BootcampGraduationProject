using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShoppingListApp.ViewModels;

namespace ShoppingListApp.Controllers
{
    public class AdminController : ControllerBase
    {
        public IActionResult Panel()
        {
            return View();
        }

        public IActionResult Categories()
        {
            var query = context.Categories;
            return View(query.ToList());
        }

        public IActionResult Products()
        {
            var query = context.Products.Include(a => a.Category)
                .Select(b => new ProductViewModel
                {
                    ProductId = b.ProductId,
                    CategoryId = b.CategoryId,
                    CategoryName =b.Category.Name,
                    Name =b.Name,
                    Image =b.Image,
                });
            return View(query.ToList());
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
