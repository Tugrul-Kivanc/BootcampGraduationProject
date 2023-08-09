using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ShoppingListApp.ViewModels;
using ShoppingListModel.Models;

namespace ShoppingListApp.Controllers
{
    [Route("[controller]/[action]")]
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
            var products = context.Products.Include(a => a.Category)
                            .Select(b => new ProductViewModel
                            {
                                ProductId = b.ProductId,
                                CategoryId = b.CategoryId,
                                CategoryName = b.Category.Name,
                                Name = b.Name,
                                Image = b.Image,
                            }).ToList();

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

                Category category = new Category();
                category.Name = categoryToAdd.Name; 

                context.Categories.Add(category);
                var result = context.SaveChanges();
                if (result == 0)
                    throw new Exception("No changes were made to the database");

                return RedirectToAction(nameof(Categories));
            }
            catch (Exception)
            {
                // TODO display error message or prevent input
                return View();
            }
        }

        [Route("{id:int}")]
        public IActionResult EditCategory(int id)
        {
            if (context.Categories == null)
                return NotFound();

            var category = context.Categories.Find(id);
            if (category == null)
                return NotFound();

            return View(category);
        }

        [HttpPost]
        [Route("{id:int}")]
        public IActionResult EditCategory(int id, Category categoryToEdit) // TODO Show current values
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
                // TODO display error message or prevent input
                return View();
            }
        }

        [Route("{id:int}")]
        public IActionResult DeleteCategory(int id)
        {
            if(context.Categories == null)
                return NotFound();

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
                    throw new Exception("Category not Found");

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
            try
            {
                var isDuplicateName = context.Products.Where(a => a.Name == productToAdd.Name).Count() > 0;
                var isSameCategory = context.Products.Where(a => a.CategoryId == productToAdd.CategoryId).Count() > 0;

                if (isDuplicateName && isSameCategory)
                    throw new Exception("Product already exists in category");

                Product product = new Product();
                product.CategoryId = productToAdd.CategoryId;
                product.Name = productToAdd.Name;
                product.Image = productToAdd.Image;

                context.Products.Add(product);
                var result = context.SaveChanges();

                if (result == 0)
                    throw new Exception("No changes were made to the database");

                return RedirectToAction(nameof(Products));
            }
            catch (Exception)
            {
                // TODO display error message or prevent input
                GenerateCategorySelectListViewBag();
                return View();
            }
        }

        [Route("{id:int}")]
        public IActionResult EditProduct(int id)
        {
            if (context.Products == null)
                return NotFound();

            var product = context.Products.Find(id);
            if (product == null)
                return NotFound();

            GenerateCategorySelectListViewBag();
            return View(product);
        }

        [HttpPost]
        [Route("{id:int}")]
        public IActionResult EditProduct(int id, Product productToEdit) // TODO Show current values
        {
            try
            {
                var isDuplicateName = context.Products.Where(a => a.Name == productToEdit.Name).Count() > 0;
                var isSameCategory = context.Products.Where(a => a.CategoryId == productToEdit.CategoryId).Count() > 0;

                if (isDuplicateName && isSameCategory)
                    throw new Exception("Product already exists");

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
                // TODO display error message or prevent input
                GenerateCategorySelectListViewBag();
                return View();
            }
        }

        [Route("{id:int}")]
        public IActionResult DeleteProduct(int id)
        {
            if (context.Products == null)
                return NotFound();

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
                    throw new Exception("Product not Found");

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
    }
}
