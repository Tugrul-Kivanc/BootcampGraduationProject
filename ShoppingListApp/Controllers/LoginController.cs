using Microsoft.AspNetCore.Mvc;
using ShoppingListApp.Extensions;
using ShoppingListApp.ViewModels;
using ShoppingListModel.Models;

namespace ShoppingListApp.Controllers
{
    public class LoginController : ControllerBase
    {
        [Route("Login")]
        public IActionResult Login()
        {
            return View();
        }

        [Route("Login")]
        [HttpPost]
        public IActionResult Login(UserLoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = context.Users.Where(a => a.Email == model.Email).SingleOrDefault();

                if (user == null)
                {
                    ModelState.AddModelError("Email", "Email not found.");
                    return View(model);
                }

                if (user.Password != model.Password)
                {
                    ModelState.AddModelError("Password", "Wrong password.");
                    return View(model);
                }

                HttpContext.Session.SetObject("User", user);

                return RedirectToAction("List", "ShoppingList", new { id = user.UserId });
            }

            return View();
        }

        [Route("Register")]
        public IActionResult Register()
        {
            return View();
        }

        [Route("Register")]
        [HttpPost]
        public IActionResult Register(UserRegisterViewModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if(model.Password != model.ConfirmPassword)
                    {
                        ModelState.AddModelError("ConfirmPassword", "Passwords do not match");
                        return View(model);
                    }

                    var isDuplicateEmail = context.Users.Where(a => a.Email == model.Email).Count() > 0;

                    if (isDuplicateEmail)
                    {
                        ModelState.AddModelError("Email", "Email exists.");
                        return View(model);
                    }

                    var newUser = new User()
                    {
                        Name = model.Name,
                        Email = model.Email,
                        Password = model.Password
                    };

                    context.Users.Add(newUser);
                    var result = context.SaveChanges();
                    if (result == 0)
                        throw new Exception("No changes were made to the database");

                    return RedirectToAction(nameof(Login));
                }

                return View(model);
            }
            catch (Exception)
            {
                return View(model);
            }
        }
    }
}
