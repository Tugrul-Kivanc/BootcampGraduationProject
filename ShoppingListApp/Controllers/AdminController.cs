using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ShoppingListApp.Extensions;
using ShoppingListApp.ViewModels;
using ShoppingListModel.Models;
using X.PagedList;

namespace ShoppingListApp.Controllers
{
    [Route("[controller]/[action]")]
    public class AdminController : ControllerBase
    {
        private int pageSize = 10;

        public IActionResult Panel()
        {
            // TODO apply authentication to every action
            User? user;
            if(!TryGetUserFromSession(out user))
            {
                return RedirectToAction("Login", "Login");
            }

            if (!user.IsAdmin)
            {
                return RedirectToAction("List", "ShoppingList");
            }

            return View();
        }

        public IActionResult Categories(int page = 1)
        {
            var categories = context.Categories.ToPagedList(page, pageSize);
            return View(categories);
        }

        public IActionResult Products(int page = 1)
        {
            var products = GetProducts().ToPagedList(page, pageSize);
            return View(products);
        }

        public IActionResult CreateCategory()
        {
            return View();
        }

        [HttpPost]
        public IActionResult CreateCategory(CategoryCreateViewModel categoryToAdd)
        {
            try
            {
                var isDuplicateName = context.Categories.Where(a => a.Name == categoryToAdd.Name).Count() > 0;
                if (isDuplicateName)
                    throw new Exception("Category already exists");

                Category category = new Category()
                {
                    Name = categoryToAdd.Name
                };

                context.Categories.Add(category);
                var result = context.SaveChanges();
                if (result == 0)
                    throw new Exception("No changes were made to the database");

                return RedirectToAction(nameof(Categories));
            }
            catch (Exception)
            {
                return View();
            }
        }

        [Route("{id:int}")]
        public IActionResult EditCategory(int id)
        {
            var category = context.Categories.Find(id);
            if (category == null)
                return RedirectToAction(nameof(Categories));

            return View(category);
        }

        [HttpPost]
        [Route("{id:int}")]
        public IActionResult EditCategory(int id, Category categoryToEdit)
        {
            try
            {
                var isDuplicateName = context.Categories.Where(a => a.Name == categoryToEdit.Name).Count() > 0;
                if (isDuplicateName)
                    throw new Exception("Category already exists");

                var category = context.Categories.Where(a => a.CategoryId == id).SingleOrDefault();
                category.Name = categoryToEdit.Name;

                var result = context.SaveChanges();
                if (result == 0)
                    throw new Exception("No changes were made to the database");

                return RedirectToAction(nameof(Categories));
            }
            catch (Exception)
            {
                return View();
            }
        }

        [Route("{id:int}")]
        public IActionResult DeleteCategory(int id)
        {
            var categoryToDelete = context.Categories.Find(id);
            if (categoryToDelete == null)
                return RedirectToAction(nameof(Categories));

            return View(categoryToDelete);
        }

        [Route("{id:int}")]
        [HttpPost, ActionName(nameof(DeleteCategory))]
        public IActionResult DeleteCategoryConfirm(int id)
        {
            try
            {
                var categoryToDelete = context.Categories.Find(id);
                if (categoryToDelete == null)
                    return RedirectToAction(nameof(Categories));

                context.Categories.Remove(categoryToDelete);
                var result = context.SaveChanges();
                if (result == 0)
                    throw new Exception("No changes were made to the database");

                return RedirectToAction(nameof(Categories));
            }
            catch (Exception)
            {
                return View();
            }
        }

        public IActionResult CreateProduct()
        {
            GenerateCategorySelectListViewBag();

            return View();
        }

        [HttpPost]
        public IActionResult CreateProduct(ProductCreateViewModel productToAdd)
        {
            GenerateCategorySelectListViewBag();
            try
            {
                Product product = new Product()
                {
                    CategoryId = productToAdd.CategoryId,
                    Name = productToAdd.Name,
                    Image = productToAdd.Image
                };

                context.Products.Add(product);
                var result = context.SaveChanges();
                if (result == 0)
                    throw new Exception("No changes were made to the database");

                return RedirectToAction(nameof(Products));
            }
            catch (Exception)
            {
                return View();
            }
        }

        [Route("{id:int}")]
        public IActionResult EditProduct(int id)
        {
            GenerateCategorySelectListViewBag();

            var product = GetProducts().Where(a => a.ProductId == id).Single();
            if (product == null)
                return RedirectToAction(nameof(Products));

            return View(product);
        }

        [HttpPost]
        [Route("{id:int}")]
        public IActionResult EditProduct(int id, ProductViewModel productToEdit)
        {
            GenerateCategorySelectListViewBag();
            try
            {
                var product = context.Products.Where(a => a.ProductId == id).SingleOrDefault();
                product.CategoryId = productToEdit.CategoryId;
                product.Name = productToEdit.Name;
                product.Image = productToEdit.Image;

                var result = context.SaveChanges();
                if (result == 0)
                    throw new Exception("No changes were made to the database");

                return RedirectToAction(nameof(Products));
            }
            catch (Exception)
            {
                return View();
            }
        }

        [Route("{id:int}")]
        public IActionResult DeleteProduct(int id)
        {
            var productToDelete = context.Products.Find(id);
            if (productToDelete == null)
                return RedirectToAction(nameof(Products));

            var query = context.Products.Where(b => b.ProductId == id).Include(b => b.Category).Single();

            return View(query);
        }

        [Route("{id:int}")]
        [HttpPost, ActionName(nameof(DeleteProduct))]
        public IActionResult DeleteProductConfirm(int id)
        {
            try
            {
                var productToDelete = context.Products.Find(id);
                if (productToDelete == null)
                    return RedirectToAction(nameof(Products));

                context.Products.Remove(productToDelete);
                var result = context.SaveChanges();
                if (result == 0)
                    throw new Exception("No changes were made to the database");

                return RedirectToAction(nameof(Products));
            }
            catch (Exception)
            {
                return View();
            }
        }

        private void GenerateCategorySelectListViewBag()
        {
            List<SelectListItem> selectList = new List<SelectListItem>();

            var query = context.Categories.Select(a => new
            {
                a.CategoryId,
                a.Name
            }).ToList();

            query.ForEach(a => selectList.Add(new SelectListItem() { Text = a.Name, Value = a.CategoryId.ToString() }));

            ViewBag.Categories = selectList;
        }

        private IQueryable<ProductViewModel> GetProducts()
        {
            var products = from p in context.Products
                           join c in context.Categories
                           on p.CategoryId equals c.CategoryId
                           into productCategory
                           from pc in productCategory
                           select new ProductViewModel
                           {
                               ProductId = p.ProductId,
                               CategoryId = pc.CategoryId,
                               CategoryName = pc.Name,
                               Name = p.Name,
                               Image = p.Image
                           };

            return products;
        }
    }
}
