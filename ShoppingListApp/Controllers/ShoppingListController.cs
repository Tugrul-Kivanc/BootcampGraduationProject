using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ShoppingListApp.Extensions;
using ShoppingListApp.ViewModels;
using ShoppingListModel.Models;
using System.Collections.Generic;
using static Azure.Core.HttpHeader;

namespace ShoppingListApp.Controllers
{
    [Route("[controller]/[action]")]
    public class ShoppingListController : ControllerBase
    {
        public IActionResult List()
        {
            User user;
            if(!TryGetUserFromSession(out user))
            {
                HttpContext.Session.Clear();
                return RedirectToAction("Login", "Login");
            }

            var shoppingLists = context.ShoppingLists.Where(a => a.UserId == user.UserId);
            return View(shoppingLists.ToList());
        }

        [Route("{id:int}")]
        public IActionResult ListDetails(int id) // Takes Shopping List Id
        {
            ViewBag.ListName = context.ShoppingLists.Find(id).ShoppingListName;

            var productList = GetListDetails(id);

            return View(productList.ToList());
        }

        public IActionResult CreateList()
        {
            return View();
        }

        [HttpPost]
        public IActionResult CreateList(ShoppingList listToCreate)
        {
            User user;
            if (!TryGetUserFromSession(out user))
            {
                HttpContext.Session.Clear();
                return RedirectToAction("Login", "Login");
            }

            try
            {
                ShoppingList shoppingList = new ShoppingList()
                {
                    ShoppingListName = listToCreate.ShoppingListName,
                    UserId = user.UserId
                };

                context.ShoppingLists.Add(shoppingList);
                var result = context.SaveChanges();
                if (result == 0)
                    throw new Exception("No changes were made to the database");

                return RedirectToAction(nameof(List));
            }
            catch (Exception)
            {
                return View();
            }
        }

        [Route("{id:int}")]
        public IActionResult EditList(int id) // Takes Shopping List Id
        {
            var shoppingList = context.ShoppingLists.Find(id);

            if (shoppingList == null)
                return RedirectToAction(nameof(List));

            return View(shoppingList);
        }

        [HttpPost]
        [Route("{id:int}")]
        public IActionResult EditList(ShoppingList shoppingListToEdit)
        {
            try
            {
                var shoppingList = context.ShoppingLists.Find(shoppingListToEdit.ShoppingListId);
                shoppingList.ShoppingListName = shoppingListToEdit.ShoppingListName;

                var result = context.SaveChanges();
                if (result == 0)
                    throw new Exception("No changes were made to the database");

                return RedirectToAction(nameof(List));
            }
            catch (Exception)
            {
                return View();
            }
        }

        [Route("{id:int}")]
        public IActionResult DeleteList(int id)
        {
            var shoppingListToDelete = context.ShoppingLists.Find(id);

            if (shoppingListToDelete == null)
                return RedirectToAction(nameof(List));

            return View(shoppingListToDelete);
        }

        [Route("{id:int}")]
        [HttpPost, ActionName(nameof(DeleteList))]
        public IActionResult DeleteListConfirm(int id)
        {
            try
            {
                var shoppingListToDelete = context.ShoppingLists.Find(id);

                if (shoppingListToDelete == null)
                    return RedirectToAction(nameof(List));

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

        [Route("{listId:int}")]
        public IActionResult AddProduct(int listId)
        {
            GenerateProductSelectListViewBag();
            return View();
        }

        [HttpPost]
        [Route("{listId:int}")]
        public IActionResult AddProduct(ShoppingListDetail productToAdd)
        {
            GenerateProductSelectListViewBag();
            try
            {
                ShoppingListDetail listDetail = new ShoppingListDetail()
                {
                    ProductId = productToAdd.ProductId,
                    Product = context.Products.Where(a => a.ProductId == productToAdd.ProductId).SingleOrDefault(),
                    ShoppingListId = productToAdd.ShoppingListId,
                    Note = productToAdd.Note,
                    Quantity = productToAdd.Quantity
                };

                context.ShoppingListDetails.Add(listDetail);
                var result = context.SaveChanges();
                if (result == 0)
                    throw new Exception("No changes were made to the database");

                return RedirectToAction(nameof(ListDetails), new {id = productToAdd.ShoppingListId});
            }
            catch (Exception)
            {
                return View();
            }
        }

        [Route("{listId:int}/{productId:int}")]
        public IActionResult EditProduct(int listId, int productId)
        {
            var model = GetShoppingListViewModel(listId, productId);

            if(model == null)
                return RedirectToAction(nameof(ListDetails), new { id = listId });

            return View(model);
        }

        [HttpPost]
        [Route("{listId:int}/{productId:int}")]
        public IActionResult EditProduct(ShoppingListViewModel model)
        {
            try
            {
                var listDetails = context.ShoppingListDetails.Where(a => a.ShoppingListId == model.ShoppingListId && a.ProductId == model.ProductId).Single();
                listDetails.Quantity = model.Quantity <= 0 ? 1 : model.Quantity;
                listDetails.Note = model.Notes;

                context.Update(listDetails);
                var result = context.SaveChanges();
                if (result == 0)
                    throw new Exception("No changes were made to the database");

                return RedirectToAction(nameof(ListDetails), new { id = model.ShoppingListId });
            }
            catch (Exception)
            {
                return View();
            }
        }

        [Route("{listId:int}/{productId:int}")]
        public IActionResult RemoveProduct(int listId, int productId)
        {
            var model = GetShoppingListViewModel(listId, productId);

            if (model == null)
                return RedirectToAction(nameof(ListDetails), new { id = listId });

            return View(model);
        }

        [HttpPost]
        [Route("{listId:int}/{productId:int}")]
        public IActionResult RemoveProduct(ShoppingListViewModel model)
        {
            try
            {
                var productToRemove = context.ShoppingListDetails.Where(a => a.ShoppingListId == model.ShoppingListId && a.ProductId == model.ProductId).Single();

                context.ShoppingListDetails.Remove(productToRemove);
                var result = context.SaveChanges();
                if (result == 0)
                    throw new Exception("No changes were made to the database");

                return RedirectToAction(nameof(ListDetails), new { id = model.ShoppingListId });
            }
            catch (Exception)
            {
                return View();
            }
        }

        [Route("{listId:int}")]
        public IActionResult Shop(int listId)
        {
            ViewBag.ListName = context.ShoppingLists.Find(listId).ShoppingListName;

            var shoppingList = GetListDetails(listId);
            HttpContext.Session.SetObject("ShoppingList", shoppingList);

            return View(shoppingList);
        }

        [HttpPost]
        [Route("{listId:int}/{productId:int}")]
        [ActionName(nameof(Shop))]
        public IActionResult ShopUpdate(int listId, int productId)
        {
            var shoppingList = HttpContext.Session.GetObject<List<ShoppingListViewModel>>("ShoppingList");
            var productToRemove = shoppingList.Where(a => a.ShoppingListId == listId && a.ProductId == productId).Single();
            shoppingList.Remove(productToRemove);
            return View(shoppingList);
        }

        private List<ShoppingListViewModel> GetListDetails(int id)
        {
            var productList = from d in context.ShoppingListDetails
                              join p in context.Products
                              on d.ProductId equals p.ProductId
                              into listDetails
                              from ld in listDetails.DefaultIfEmpty()
                              where d.ShoppingListId == id
                              select new ShoppingListViewModel
                              {
                                  ProductId = ld.ProductId,
                                  Image = ld.Image,
                                  Name = ld.Name,
                                  Quantity = d.Quantity,
                                  Notes = d.Note
                              };

            return productList.ToList();
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

        private ShoppingListViewModel GetShoppingListViewModel(int listId, int productId)
        {
            var productToEdit = context.Products.Find(productId);
            var productInList = context.ShoppingListDetails.Where(a => a.ShoppingListId == listId && a.ProductId == productId);

            if (productToEdit == null || productInList.Count() == 0)
                return null;

            var model = new ShoppingListViewModel()
            {
                Image = productToEdit.Image,
                Name = productToEdit.Name,
                ProductId = productId,
                Quantity = productInList.Select(b => b.Quantity).Single(),
                Notes = productInList.Select(b => b.Note).Single()
            };

            return model;
        }
    }
}
