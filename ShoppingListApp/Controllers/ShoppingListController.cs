using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
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
            return View(shoppingLists.ToList());
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
                            ProductId = ld.ProductId,
                            Image = ld.Product.Image,
                            Name = ld.Product.Name,
                            Quantity = ld.Quantity,
                            Notes = ld.Note
                        };

            ViewBag.ShoppingListId = id;

            return View(query.ToList());
        }

        [HttpGet]
        public IActionResult CreateList()
        {
            return View();
        }

        [HttpPost]
        public IActionResult CreateList(ShoppingList listToCreate)
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

        public IActionResult DeleteList(int? id)
        {
            if (id == null || context.ShoppingLists == null)
                return NotFound();

            var shoppingListToDelete = context.ShoppingLists.Find(id);

            if (shoppingListToDelete == null)
                throw new Exception("Shopping List not Found");

            return View(shoppingListToDelete);
        }

        [HttpPost, ActionName(nameof(DeleteList))]
        public IActionResult DeleteListConfirm(int? id)
        {
            try
            {
                var shoppingListToDelete = context.ShoppingLists.Find(id);

                if (shoppingListToDelete == null)
                    throw new Exception("Shopping List Not Found");

                context.ShoppingLists.Remove(shoppingListToDelete);
                var result = context.SaveChanges();
                if (result == 0)
                    throw new Exception("No changes were made to the database");

                return RedirectToAction(nameof(DeleteList));
            }
            catch (Exception)
            {
                return View();
            }
        }

        public IActionResult AddProduct()
        {
            GenerateProductSelectListViewBag();
            return View();
        }

        [HttpPost]
        public IActionResult AddProduct(ShoppingListDetail productToAdd)
        {
            try
            {
                ShoppingListDetail listDetail = new ShoppingListDetail();
                listDetail.ProductId = productToAdd.ProductId;
                listDetail.Product = context.Products.Where(a => a.ProductId == productToAdd.ProductId).SingleOrDefault();
                listDetail.ShoppingListId = productToAdd.ShoppingListId;
                listDetail.Note = productToAdd.Note;
                listDetail.Quantity = productToAdd.Quantity;

                context.ShoppingListDetails.Add(listDetail);
                var result = context.SaveChanges();
                if (result == 0)
                    throw new Exception("No changes were made to the database");

                return RedirectToAction(nameof(ListDetails));
            }
            catch (Exception)
            {
                // TODO display error message or prevent input
                return View();
            }
        }

        public IActionResult EditProduct(int id, int listid)
        {
            var product = context.Products.Find(id);

            var model = new ShoppingListViewModel()
            {
                Image = product.Image,
                Name = product.Name,
                ProductId = product.ProductId,
                Quantity = context.ShoppingListDetails.Where(a => a.ShoppingListId == listid && a.ProductId == id).Select(b => b.Quantity).Single(),
                Notes = context.ShoppingListDetails.Where(a => a.ShoppingListId == listid && a.ProductId == id).Select(b => b.Note).Single()
            };

            return View(model);
        }

        [HttpPost]
        public IActionResult EditProduct(int id, int listid, ShoppingListViewModel model)
        {
            var listDetails = context.ShoppingListDetails.Where(a => a.ShoppingListId == listid && a.ProductId == id).Single();
            listDetails.Quantity = model.Quantity;
            listDetails.Note = model.Notes;

            context.Update(listDetails);
            context.SaveChanges();

            return RedirectToAction(nameof(ListDetails),listid);
        }

        public IActionResult DeleteProduct()
        {
            return View();
        }

        public IActionResult Shop()
        {
            return View();
        }

        private void GenerateProductSelectListViewBag()
        {
            List<SelectListItem> selectList = new List<SelectListItem>();

            var query = context.Products.Select(a => new
            {
                a.ProductId,
                a.Name
            }).ToList();

            query.ForEach(a => selectList.Add(new SelectListItem() { Text = a.Name, Value = a.ProductId.ToString() }));

            ViewBag.Products = selectList;
        }
    }
}
