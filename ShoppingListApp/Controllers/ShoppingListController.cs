using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ShoppingListApp.ViewModels;
using ShoppingListModel.Models;
using System.Collections.Generic;
using static Azure.Core.HttpHeader;

namespace ShoppingListApp.Controllers
{
    [Route("[controller]/[action]")]
    public class ShoppingListController : ControllerBase
    {
        [Route("{id:int}")]
        public IActionResult List(int id) // Takes User Id
        {
            var shoppingLists = context.ShoppingLists.Where(a => a.UserId == id);
            return View(shoppingLists.ToList());
        }

        [Route("{id:int}")]
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

        [Route("{id:int}")]
        public IActionResult CreateList(int id) //Takes user id
        {
            return View();
        }

        [HttpPost]
        [Route("{id:int}")]
        public IActionResult CreateList(int id, ShoppingList listToCreate)
        {

            try
            {
                var isDuplicateName = context.ShoppingLists.Where(a => a.ShoppingListName == listToCreate.ShoppingListName).Count() > 0;

                if (isDuplicateName)
                    throw new Exception("Shopping List Already Exists");

                ShoppingList shoppingList = new ShoppingList();
                shoppingList.ShoppingListName = listToCreate.ShoppingListName;
                shoppingList.UserId = id;

                context.ShoppingLists.Add(shoppingList);
                var result = context.SaveChanges();
                if (result == 0)
                    throw new Exception("No changes were made to the database");

                return RedirectToAction(nameof(List), new { id = id });
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
                return NotFound();

            return View(shoppingList);
        }

        [HttpPost]
        [Route("{id:int}")]
        public IActionResult EditList(ShoppingList shoppingListToEdit)
        {
            try
            {
                var isDuplicateName = context.ShoppingLists.Where(a => a.ShoppingListName == shoppingListToEdit.ShoppingListName).Count() > 0;

                if (isDuplicateName)
                    throw new Exception("Shopping List already exists");

                var shoppingList = context.ShoppingLists.Find(shoppingListToEdit.ShoppingListId);
                shoppingList.ShoppingListName = shoppingListToEdit.ShoppingListName;

                var result = context.SaveChanges();

                if (result == 0)
                    throw new Exception("No changes were made to the database");

                return RedirectToAction(nameof(List), new { id = 1 });
            }
            catch (Exception)
            {
                return View();
            }
        }

        [Route("{id:int}")]
        public IActionResult DeleteList(int id)
        {
            if (context.ShoppingLists == null)
                return NotFound();

            var shoppingListToDelete = context.ShoppingLists.Find(id);

            if (shoppingListToDelete == null)
                return RedirectToAction(nameof(List), new { id = 1 }); //TODO change id to take userid. Probably use session

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

                return RedirectToAction(nameof(ListDetails), new {id = productToAdd.ShoppingListId});
            }
            catch (Exception)
            {
                // TODO display error message or prevent input
                return View();
            }
        }

        [Route("{listId:int}/{productId:int}")]
        public IActionResult EditProduct(int listId, int productId)
        {
            var product = context.Products.Find(productId);
            var productInList = context.ShoppingListDetails.Where(a => a.ShoppingListId == listId && a.ProductId == productId);

            if (product == null || productInList.Count() == 0)
                return RedirectToAction(nameof(ListDetails), new { id = listId });

            var model = new ShoppingListViewModel();
            model.Image = product.Image;
            model.Name = product.Name;
            model.ProductId = product.ProductId;
            model.Quantity = productInList.Select(b => b.Quantity).Single();
            model.Notes = productInList.Select(b => b.Note).Single();

            return View(model);
        }

        [HttpPost]
        [Route("{listId:int}/{productId:int}")]
        public IActionResult EditProduct(ShoppingListViewModel model)
        {
            var listDetails = context.ShoppingListDetails.Where(a => a.ShoppingListId == model.ShoppingListId && a.ProductId == model.ProductId).Single();
            listDetails.Quantity = model.Quantity;
            listDetails.Note = model.Notes;

            context.Update(listDetails);
            context.SaveChanges();

            return RedirectToAction(nameof(ListDetails), new { id = model.ShoppingListId });
        }

        [Route("{listId:int}/{productId:int}")]
        public IActionResult RemoveProduct(int listId, int productId)
        {
            var productToRemove = context.Products.Find(productId);
            var productInList = context.ShoppingListDetails.Where(a => a.ShoppingListId == listId && a.ProductId == productId);

            if (productToRemove == null || productInList.Count() == 0)
                return RedirectToAction(nameof(ListDetails), new { id = listId });

            var model = new ShoppingListViewModel();
            model.Image = productToRemove.Image;
            model.Name = productToRemove.Name;
            model.ProductId = productToRemove.ProductId;
            model.Quantity = productInList.Select(b => b.Quantity).Single();
            model.Notes = productInList.Select(b => b.Note).Single();

            return View(model);
        }

        [HttpPost]
        [Route("{listId:int}/{productId:int}")]
        public IActionResult RemoveProduct(ShoppingListViewModel model)
        {
            var productToRemove = context.ShoppingListDetails.Where(a => a.ShoppingListId == model.ShoppingListId && a.ProductId == model.ProductId).Single();

            context.ShoppingListDetails.Remove(productToRemove);
            context.SaveChanges();

            return RedirectToAction(nameof(ListDetails), new { id = model.ShoppingListId });
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
