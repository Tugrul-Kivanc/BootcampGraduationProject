using Microsoft.AspNetCore.Mvc;

namespace ShoppingListApp.Controllers
{
    [Route("[controller]/[action]")]
    public class LoginController : ControllerBase
    {
        [Route("")]
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Register()
        {
            return View();
        }
    }
}
