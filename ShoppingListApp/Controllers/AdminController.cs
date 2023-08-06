using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ShoppingListApp.ViewModels;
using ShoppingListModel.Models;

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

        public IActionResult EditCategory()
        {
            return View();
        }

        [HttpPost]
        public IActionResult EditCategory(Category categoryToEdit) // TODO Show current values
        {
            try
            {
                var isDuplicateName = context.Categories.Where(a => a.Name == categoryToEdit.Name).Count() > 0;

                if (isDuplicateName)
                    throw new Exception("Category already exists");

                var category = context.Categories.Where(a => a.CategoryId == categoryToEdit.CategoryId).SingleOrDefault();
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

        public IActionResult DeleteCategory()
        {
            return View();
        }

        public IActionResult CreateProduct()
        {
            MakeCategorySelectListViewBag();

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
                MakeCategorySelectListViewBag();
                return View();
            }
        }

        public IActionResult EditProduct()
        {
            MakeCategorySelectListViewBag();
            return View();
        }

        [HttpPost]
        public IActionResult EditProduct(Product productToEdit) // TODO Show current values
        {
            try
            {
                var isDuplicateName = context.Products.Where(a => a.Name == productToEdit.Name).Count() > 0;
                var isSameCategory = context.Products.Where(a => a.CategoryId == productToEdit.CategoryId).Count() > 0;

                if (isDuplicateName && isSameCategory)
                    throw new Exception("Product already exists");

                var product = context.Products.Where(a => a.ProductId == productToEdit.ProductId).SingleOrDefault();
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
                MakeCategorySelectListViewBag();
                return View();
            }
        }

        public IActionResult RemoveProduct()
        {
            return View();
        }

        private void MakeCategorySelectListViewBag()
        {
            List<SelectListItem> selectList = new List<SelectListItem>();

            var query = context.Categories.Select(a => new
            {
                a.CategoryId,
                a.Name
            }).ToList();

            //foreach(var item in query)
            //{
            //    selectList.Add(new SelectListItem() { Text = item.Name, Value = item.CategoryId.ToString() });
            //}

            query.ForEach(a => selectList.Add(new SelectListItem() { Text = a.Name, Value = a.CategoryId.ToString() }));

            ViewBag.Categories = selectList;
        }
    }
}
