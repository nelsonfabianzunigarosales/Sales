using Microsoft.AspNetCore.Mvc;

namespace Auth0net.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
