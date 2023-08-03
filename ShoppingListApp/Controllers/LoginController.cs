using Microsoft.AspNetCore.Mvc;

namespace ShoppingListApp.Controllers
{
    public class LoginController : ControllerBase
    {
        public IActionResult Login()
        {
            return View();
        }
        public IActionResult Register()
        {
            return View();
        }
    }
}
