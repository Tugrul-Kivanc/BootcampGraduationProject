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

        public IActionResult DeleteList()
        {
            return View();
        }

        public IActionResult AddProduct()
        {
            MakeProductSelectListViewBag();
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

        public IActionResult EditProduct()
        {
            return View();
        }

        public IActionResult DeleteProduct()
        {
            return View();
        }

        public IActionResult Shop()
        {
            return View();
        }

        private void MakeProductSelectListViewBag()
        {
            List<SelectListItem> selectList = new List<SelectListItem>();

            var query = context.Products.Select(a => new
            {
                a.ProductId,
                a.Name
            }).ToList();

            //foreach(var item in query)
            //{
            //    selectList.Add(new SelectListItem() { Text = item.Name, Value = item.CategoryId.ToString() });
            //}

            query.ForEach(a => selectList.Add(new SelectListItem() { Text = a.Name, Value = a.ProductId.ToString() }));

            ViewBag.Products = selectList;
        }
    }
}
