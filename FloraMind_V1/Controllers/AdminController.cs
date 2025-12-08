using Microsoft.AspNetCore.Mvc;

namespace FloraMind_V1.Controllers
{
    public class AdminController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
